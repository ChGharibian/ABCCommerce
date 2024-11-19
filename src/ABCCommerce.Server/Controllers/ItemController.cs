using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using System.Data;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class ItemController : Controller
{
    public ILogger<ItemController> Logger { get; }
    public ABCCommerceContext AbcDb { get; }

    public ItemController(ILogger<ItemController> logger, ABCCommerceContext abcDb)
    {
        Logger = logger;
        AbcDb = abcDb;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetItemQuery([FromQuery] string sku)
    {
        return Ok(AbcDb.Items.Where(i => i.SKU == sku).Include(i => i.Listings).Select(i => i.ToDto()).ToArray());

    }

    [HttpGet("{item:int}")]
    public ActionResult<IEnumerable<Item>> GetItems(int item)
    {
        return Ok(AbcDb.Items.Where(i => i.Id == item).Include(i => i.Listings).Select(i => i.ToDto()).ToArray());
    }

    [HttpPost]
    public async Task<ActionResult<Item>> CreateItem([FromBody] CreateItemRequest createItem)
    {
        var seller = await AbcDb.Sellers.FirstOrDefaultAsync(s => s.Id == createItem.Seller);
        if(seller is null)
        {
            return BadRequest(new
            {
                Error = "Seller Not Found"
            });
        }
        var item = new ABCCommerceDataAccess.Models.Item
        {
            SKU = createItem.Sku,
            Name = createItem.Name,
        };
        seller.Items.Add(item);
        await AbcDb.SaveChangesAsync();
        return Ok(item.ToDto());
    }
}
