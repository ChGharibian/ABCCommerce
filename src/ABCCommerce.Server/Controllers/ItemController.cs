using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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



    [HttpGet("{item:int}")]
    public ActionResult<IEnumerable<Item>> GetItems(int item)
    {
        return Ok(AbcDb.Items.Where(i => i.Id == item).Include(i => i.Listings).Select(i => new { i.Id, i.Name, i.SKU, i.Listings }).ToArray());
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
        var item = new Item
        {
            SKU = createItem.Sku,
            Name = createItem.Name,
        };
        seller.Items.Add(item);
        await AbcDb.SaveChangesAsync();
        return Ok(new { item.Id, item.Name, item.SKU });
    }
}
public class CreateItemRequest
{
    public int Seller { get; set; }
    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
}