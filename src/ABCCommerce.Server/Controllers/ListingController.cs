
using ABCCommerceDataAccess;
using Examine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ListingController : Controller
{
    public ILogger<ListingController> Logger { get; }
    public ABCCommerceContext AbcDb { get; }
    public IExamineManager ExamineManager { get; }
    public IHostEnvironment HostEnvironment { get; }

    public ListingController(ILogger<ListingController> logger, ABCCommerceContext abcDb, IExamineManager examineManager, IHostEnvironment hostEnvironment)
    {
        Logger = logger;
        AbcDb = abcDb;
        ExamineManager = examineManager;
        HostEnvironment = hostEnvironment;
    }
    [HttpPost("{listing:int}/Image")]
    public ActionResult<string> AddImageToListing([FromQuery] int listing, [FromBody] AddImageRequest imageRequest)
    {
        var editListing = AbcDb.Listings.Where(l => l.Id == listing).Include(l => l.Images).FirstOrDefault();
        if (editListing is null) return NotFound();
        var bytes = Convert.FromBase64String(imageRequest.Image);
        string name = Path.Combine("listings", $"{DateTime.Now:O}.{imageRequest.FileType}");
        string pathstart = Path.Combine(HostEnvironment.ContentRootPath, "images");
        System.IO.File.WriteAllBytes(Path.Combine(pathstart, name), bytes);
        editListing.Images.Add(new ABCCommerceDataAccess.Models.ListingImage() { Image = name });
        AbcDb.SaveChanges();
        return Ok(new ImagePath(name));
    }

    [HttpGet("{seller:int}")]
    public async Task<ActionResult<Listing>> GetListings(int seller)
    {
        return Ok(
            await AbcDb.Listings
            .Include(l => l.Item)
            .Include(l => l.Images)
            .Where(l => l.Item.SellerId == seller)
            .Where(l => l.Active)
            .Select(l => new { l.Id, l.ListingDate, l.Item, l.Tags, l.PricePerUnit, l.Quantity, l.Active, Image = l.Images.Select(i => new ImagePath(i.Image)) })
            .ToArrayAsync());
    }
    [HttpPost()]
    public async Task<ActionResult<Listing>> CreateListing([FromBody] CreateListingRequest createListing)
    {
        var item = await AbcDb.Items.Include(i => i.Listings).Include(i => i.Seller).Where(i => i.Id == createListing.Item).FirstOrDefaultAsync();
        if (item is null)
        {
            return NotFound();
        }
        var listing = new ABCCommerceDataAccess.Models.Listing
        {
            Name = createListing.Name,
            Active = createListing.Active,
            ListingDate = DateTime.UtcNow,
            Description = createListing.Description,
            PricePerUnit = createListing.Price,
            Quantity = createListing.Quantity,
            Tags = createListing.Tags.ToArray(),
        };
        item.Listings.Add(listing);
        await AbcDb.SaveChangesAsync();
        ExamineManager.GetIndex("MyIndex").Index(listing);
        return Ok(listing.ToDto());
    }
    [HttpPatch("{listing:int}")]
    public async Task<ActionResult<Listing>> UpdateListing([FromQuery] int listing, [FromBody] UpdateListingRequest updateRequest)
    {
        var editListing = AbcDb.Listings.Where(l => l.Id == listing).Include(l => l.Images).FirstOrDefault();
        if (editListing is null) return NotFound();
        if(updateRequest.Active is bool b)
        {
            editListing.Active = b;
        }
        if(updateRequest.Price is decimal d)
        {
            editListing.PricePerUnit = d;
        }
        if(updateRequest.Description is string s)
        {
            editListing.Description = s;
        }
        if(updateRequest.Quantity is int i)
        {
            editListing.Quantity = i;
        }
        if(updateRequest.RemoveTags is not null || updateRequest.AddTags is not null)
        {
            editListing.Tags = editListing.Tags.Except(updateRequest.RemoveTags ?? Array.Empty<string>()).Concat(updateRequest.AddTags ?? Array.Empty<string>()).ToArray();
        }
        AbcDb.SaveChanges();
        return editListing.ToDto();
    }
}
