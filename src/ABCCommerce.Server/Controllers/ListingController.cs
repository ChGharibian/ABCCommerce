
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

    public ListingController(ListingService listingService, ILogger<ListingController> logger, ABCCommerceContext abcDb)
    {
        ListingService = listingService;
        Logger = logger;
        AbcDb = abcDb;
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
            .Where(l => l.Id == listingId)
            .Where(l => l.Active)
            .Select(l => l.ToDto())
            .FirstOrDefaultAsync();
        if (listing is null) return NotFound();
        return Ok(listing);
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
    /// Get the listings belonging to the seller.
    /// </summary>
    /// <param name="sellerId">The id of the seller the listings belong to.</param>
    /// <param name="skip">The number of items to skip. Used for paging.</param>
    /// <param name="count">The number of items to return from a search. Maximum of 50.</param>
    /// <returns></returns>
    [HttpGet(Name = "Get Listings")]
    public async Task<ActionResult<IEnumerable<Listing>>> GetListings([FromQuery] int? skip, [FromQuery] int? count, [FromQuery] int? sellerId)
    {
        var listingQuery = AbcDb.Listings
                    .Include(l => l.Item)
                    .Include(l => l.Images)
                    .Where(l => l.Active);
        if(sellerId is int s)
        {

            listingQuery = listingQuery.Where(l => l.Item.SellerId == sellerId);
        }
        var listings = await listingQuery
                    .Skip(skip ?? 0)
                    .Take(Math.Min(50, count ?? 50))
                    .Select(l => l.ToDto())
                    .ToArrayAsync();
        return Ok(listings);
    }
}
