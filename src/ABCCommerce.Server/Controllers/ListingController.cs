
using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ListingController : Controller
{
    public ILogger<ListingController> Logger { get; }
    public ABCCommerceContext AbcDb { get; }
    public IHostEnvironment HostEnvironment { get; }

    public ListingController(ILogger<ListingController> logger, ABCCommerceContext abcDb, IHostEnvironment hostEnvironment)
    {
        Logger = logger;
        AbcDb = abcDb;
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
        editListing.Images.Add(new ListingImage() { Image = name });
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
        var listing = new Listing
        {
            Active = createListing.Active,
            ListingDate = DateTime.UtcNow,
            Description = createListing.Description,
            PricePerUnit = createListing.Price,
            Quantity = createListing.Quantity,
            Tags = createListing.Tags.ToArray(),
        };
        item.Listings.Add(listing);
        await AbcDb.SaveChangesAsync();
        return Ok(new { listing.Id, listing.Active, listing.Description, listing.ListingDate, listing.PricePerUnit, listing.Quantity, listing.Tags });
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
        return editListing;
    }
}
public class CreateListingRequest
{
    public int Item { get; set; }
    public int Quantity { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
    public decimal Price { get; set; }
    public IEnumerable<string> Tags { get; set; } = [];
}
public class UpdateListingRequest
{
    public int? Quantity { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    public bool? Active { get; set; }
    public decimal? Price { get; set; }
    public string[]? AddTags { get; set; }
    public string[]? RemoveTags { get; set; }
}
public class AddImageRequest
{
    [Required]
    public string Image { get; set; } = "";
    [Required]
    [MaxLength(10)]
    public string FileType { get; set; } = "";
}