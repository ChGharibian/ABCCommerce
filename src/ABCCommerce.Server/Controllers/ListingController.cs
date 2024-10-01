
using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ListingController : Controller
{
    public ILogger<ListingController> Logger { get; }
    public ABCCommerceContext AbcDb { get; }

    public ListingController(ILogger<ListingController> logger, ABCCommerceContext abcDb)
    {
        Logger = logger;
        AbcDb = abcDb;
    }

    
    [HttpGet("{seller:int}")]
    public async Task<ActionResult<Listing>> GetListings(int seller)
    {
        return Ok(
            await AbcDb.Listings
            .Include(l => l.Item)
            .Where(l => l.Item.SellerId == seller)
            .Where(l => l.Active)
            .Select(l => new {l.Id, l.ListingDate, l.Item, l.Tags, l.PricePerUnit, l.Quantity, l.Active})
            .ToArrayAsync());
    }
    [HttpPost()]
    public async Task<ActionResult<Listing>> CreateListing([FromBody] CreateListingRequest createListing)
    {
        var item = await AbcDb.Items.Include(i => i.Listings).Include(i => i.Seller).Where(i => i.Id == createListing.Item).FirstOrDefaultAsync();
        if(item is null)
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