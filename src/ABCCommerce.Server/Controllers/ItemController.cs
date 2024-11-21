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
    public ActionResult<Item> GetItemQuery([FromQuery] string sku)
    {
        var dto = AbcDb.Items.Where(i => i.SKU == sku).Include(i => i.Listings).Select(i => i.ToDto()).FirstOrDefault();
        if(dto is null)
        {
            return NotFound();
        }
        return Ok(dto);

    }

    [HttpGet("exists")]
    public ActionResult<ItemExists> GetItemExists([FromQuery] string sku)
    {
        bool exists = AbcDb.Items.Where(i => i.SKU == sku).Any();
        return Ok(new ItemExists(
exists
        ));
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
        if(AbcDb.Items.Any(i => i.SellerId == seller.Id && i.SKU == createItem.Sku))
        {
            return Problem("Item sku already exists.", statusCode: StatusCodes.Status400BadRequest);
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

public record ItemExists(bool Exists);
