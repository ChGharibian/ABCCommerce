using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using System.Security.Claims;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class SellerController : ControllerBase
{

    private readonly ILogger<SellerController> _logger;
    public ABCCommerceContext ABCDb { get; }

    public SellerController(ILogger<SellerController> logger, ABCCommerceContext abcDb)
    {
        _logger = logger;
        ABCDb = abcDb;
    }

    /// <summary>
    /// Get all sellers.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "Get Sellers")]
    public ActionResult<IEnumerable<Seller>> Get()
    {
        return Ok(ABCDb.Sellers.Select(s => s.ToDto()).ToArray());
    }
    /// <summary>
    /// Request a seller using the provided seller id.
    /// </summary>
    /// <param name="sellerId">The id of the requested seller.</param>
    /// <returns></returns>
    [HttpGet("{sellerId:int}", Name = "Get Seller")]
    public async Task<ActionResult<Seller>> GetSeller(int sellerId)
    {
        var seller = await ABCDb.Sellers
            .Where(s => s.Id == sellerId)
            .FirstOrDefaultAsync();
        if(seller is null)
        {
            return NotFound();
        }
        return Ok(seller.ToDto());
    }
    /// <summary>
    /// Add a new seller.
    /// </summary>
    /// <param name="sellerCreate"></param>
    /// <returns></returns>
    [HttpPost(Name = "Add Seller")]
    public async Task<ActionResult<Seller>> AddSeller([FromBody] SellerCreateRequest sellerCreate)
    {
        var seller = new ABCCommerceDataAccess.Models.Seller { Name = sellerCreate.Name };
        ABCDb.Sellers.Add(seller);
        await ABCDb.SaveChangesAsync();
        return Ok(seller);
    }
    /// <summary>
    /// Get the listings belonging to the seller.
    /// </summary>
    /// <param name="sellerId">The id of the seller the listings belong to.</param>
    /// <param name="skip">The number of items to skip. Used for paging.</param>
    /// <param name="count">The number of items to return from a search. Maximum of 50.</param>
    /// <returns></returns>
    [HttpGet("{sellerId:int}/Listings", Name = "Get Seller Listings")]
    public async Task<ActionResult<Listing>> GetListings(int sellerId, [FromQuery] int? skip, [FromQuery] int? count)
    {
        Listing[] listings = await ABCDb.Listings
                    .Include(l => l.Item)
                    .Include(l => l.Images)
                    .Where(l => l.Item.SellerId == sellerId)
                    .Where(l => l.Active)
                    .Skip(skip ?? 0)
                    .Take(Math.Min(50, count ?? 50))
                    .Select(l => l.ToDto())
                    .ToArrayAsync();
        return base.Ok(listings);
    }
    /// <summary>
    /// Get the items belonging to the seller.
    /// </summary>
    /// <param name="sellerId">The id of the seller the items belong to.</param>
    /// <returns></returns>
    [HttpGet("{sellerId:int}/Items", Name = "Get Seller Items")]
    public ActionResult<IEnumerable<Item>> GetItems(int sellerId)
    {
        return Ok(ABCDb.Items.Where(i => i.SellerId == sellerId).Include(i => i.Listings).Select(i => i.ToDto()).ToArray());
    }

    /// <summary>
    /// Used to query seller items based on the sku.
    /// </summary>
    /// <param name="sellerId">The seller the item belongs to.</param>
    /// <param name="sku">The sku of the item.</param>
    /// <repsonse code="200">Returned item.</repsonse>
    /// <repsonse code="404">Item sku does not belond the the seller.</repsonse>
    [HttpGet("{sellerId:int}/Items/{sku}", Name = "Get Seller Item")]
    public ActionResult<IEnumerable<Item>> GetItem(int sellerId, string sku)
    {
        var item = ABCDb.Items.Where(i => i.SellerId == sellerId && i.SKU == sku).Include(i => i.Listings).Select(i => i.ToDto()).FirstOrDefault();
        if (item is null) return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Check if the item sku is already being used by the seller.
    /// </summary>
    /// <param name="sellerId">The seller the item belongs to.</param>
    /// <param name="sku">The sku of the item.</param>
    [HttpGet("{sellerId:int}/Items/{sku}/Exists", Name = "Get Seller Item Exists")]
    public ActionResult<ItemExists> GetItemExists(int sellerId, string sku)
    {
        bool exists = ABCDb.Items.Where(i => i.SellerId == sellerId && i.SKU == sku).Any();
        return Ok(new ItemExists(exists));
    }
}