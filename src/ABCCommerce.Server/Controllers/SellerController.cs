using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;

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


    [HttpGet(Name = "GetSellers")]
    public ActionResult<IEnumerable<Seller>> Get()
    {
        return Ok(ABCDb.Sellers.Select(s => s.ToDto()).ToArray());
    }
    [HttpGet("{id:int}", Name = "GetSeller")]
    public async Task<ActionResult<Seller>> GetSeller(int id)
    {
        var seller = await ABCDb.Sellers
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
        if(seller is null)
        {
            return NotFound();
        }
        return Ok(seller.ToDto());
    }
    [HttpPost(Name = "AddSeller")]
    public async Task<ActionResult<Seller>> AddSeller([FromBody] SellerCreateRequest sellerCreate)
    {
        var seller = new ABCCommerceDataAccess.Models.Seller { Name = sellerCreate.Name };
        ABCDb.Sellers.Add(seller);
        await ABCDb.SaveChangesAsync();
        return Ok(seller);
    }

    [HttpGet("{seller:int}/Items")]
    public ActionResult<IEnumerable<Item>> GetItems(int seller)
    {
        return Ok(ABCDb.Items.Where(i => i.SellerId == seller).Include(i => i.Listings).Select(i => i.ToDto()).ToArray());
    }

    [HttpGet("{seller:int}/Items/{item:int}")]
    public ActionResult<IEnumerable<Item>> GetItems(int seller, int item)
    {
        return Ok(ABCDb.Items.Where(i => i.SellerId == seller && i.Id == item).Include(i => i.Listings).Select(i => i.ToDto()).ToArray());
    }
}