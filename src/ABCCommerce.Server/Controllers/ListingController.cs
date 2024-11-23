
using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Examine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SharedModels.Models;
using SharedModels.Models.Requests;
using System.Text;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ListingController : Controller
{
    public ListingService ListingService { get; }
    public ILogger<ListingController> Logger { get; }
    public ABCCommerceContext AbcDb { get; }
    public IImageService ImageService { get; }
    public IExamineManager ExamineManager { get; }
    public IHostEnvironment HostEnvironment { get; }

    public ListingController(ListingService listingService, ILogger<ListingController> logger, ABCCommerceContext abcDb, IImageService imageService, IExamineManager examineManager, IHostEnvironment hostEnvironment)
    {
        ListingService = listingService;
        Logger = logger;
        AbcDb = abcDb;
        ImageService = imageService;
        ExamineManager = examineManager;
        HostEnvironment = hostEnvironment;
    }
    /// <summary>
    /// Adds image to a listing.
    /// </summary>
    /// <param name="listingId">The id of the listing to add an image to.</param>
    /// <param name="imageRequest"></param>
    /// <returns></returns>
    [HttpPost("{listingId:int}/Image", Name = "Add Image To Listing")]
    public ActionResult<ImagePath> AddImageToListing(int listingId, [FromBody] AddImageRequest imageRequest)
    {
        var editListing = AbcDb.Listings.Where(l => l.Id == listingId).Include(l => l.Images).FirstOrDefault();
        if (editListing is null) 
            return NotFound();
        
        var imagePath = ImageService.AddImage(imageRequest.Image, imageRequest.FileType, "listings");

        editListing.Images.Add(new ABCCommerceDataAccess.Models.ListingImage() { Image = imagePath.Path });
        AbcDb.SaveChanges();
        return Ok(imagePath);
    }
    /// <summary>
    /// Get a listing via a listing id.
    /// </summary>
    /// <param name="listingId">The id of the listing.</param>
    /// <returns></returns>
    [HttpGet("{listingId:int}", Name = "Get Listing")]
    public async Task<ActionResult<Listing>> GetListing(int listingId)
    {
        var listing = await AbcDb.Listings
            .Include(l => l.Item)
            .Include(l => l.Images)
            .Where(l => l.Item.Id == listingId)
            .Where(l => l.Active)
            .Select(l => l.ToDto())
            .FirstOrDefaultAsync();
        if (listing is null) return NotFound();
        return Ok(listing);
    }
    /// <summary>
    /// Creates a listing.
    /// </summary>
    /// <param name="createListing"></param>
    [HttpPost(Name="Create Listing")]
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
    /// <summary>
    /// Updates a listing with the provided information.
    /// </summary>
    /// <param name="listingId">The id of the lsiting to be updated</param>
    /// <param name="updateRequest"></param>
    /// <response code="200">Successfully updated the listing.</response>
    /// <response code="400">Some information provided in the body was incorrect.</response>
    /// <response code="404">Could not find the listing.</response>
    [HttpPatch("{listingId:int}", Name = "Update Listing")]
    public async Task<ActionResult<Listing>> UpdateListing(int listingId, [FromBody] UpdateListingRequest updateRequest)
    {
        var editListing = await AbcDb.Listings.Where(l => l.Id == listingId).Include(l => l.Images).FirstOrDefaultAsync();
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
    /// <summary>
    /// Returns the quantity not yet reserved by a user.
    /// </summary>
    /// <param name="listingId">The id of the listing to check.</param>
    /// <response code="404">Listing was not found.</response>
    [HttpGet("{listingId:int}/Availability", Name = "Get Availability")]
    public async Task<ActionResult<Listing>> Availability(int listingId)
    {
        var quantity = await ListingService.Availability(listingId);

        return Ok(new
        {
            Quantity = Math.Max(quantity, 0),
        });
    }
    /// <summary>
    /// Gets all current listings.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "Get Listings")]
    public async Task<ActionResult<IEnumerable<Listing>>> GetAllListings()
    {
        return Ok(await AbcDb.Listings.Include(l => l.Item).ThenInclude(i => i.Seller).Include(i => i.Images).Select(l => l.ToDto()).ToArrayAsync());
    }
}
