using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using System.Data;

namespace ABCCommerce.Server.Controllers;
/// <summary>
/// The base controller controlling items.
/// </summary>
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
    /// <summary>
    /// Gets an item using an item id.
    /// </summary>
    /// <param name="itemId">The id of the requested item.</param>
    /// <returns></returns>
    [HttpGet("{item:int}", Name = "Get Item")]
    public ActionResult<Item> GetItem(int itemId)
    {
        Item? item = AbcDb.Items.Where(i => i.Id == itemId).Include(i => i.Listings).Select(i => i.ToDto()).FirstOrDefault();
        if (item is null) return NotFound();
        return Ok(item);
    }
}
/// <summary>
/// A Class to determine whether a database object exists.
/// </summary>
/// <param name="Exists">Does the database object exist.</param>
public record ItemExists(bool Exists);
