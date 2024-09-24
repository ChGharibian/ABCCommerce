using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        return Ok(ABCDb.Sellers.Select(s => new {s.Id, s.Name}).ToArray());
    }
    [HttpGet("{id:int}", Name = "GetSeller")]
    public async Task<ActionResult<Seller>> GetSeller(int id)
    {
        var seller = await ABCDb.Sellers
            .Where(s => s.Id == id)
            .Include(s => s.Items)
            .FirstOrDefaultAsync();
        if(seller is null)
        {
            return NotFound();
        }
        return Ok(seller);
    }
    [HttpPost(Name = "AddSeller")]
    public async Task<ActionResult<Seller>> AddSeller([FromBody] SellerCreateRequest sellerCreate)
    {
        var seller = new Seller { Name = sellerCreate.Name };
        ABCDb.Sellers.Add(seller);
        await ABCDb.SaveChangesAsync();
        return Ok(seller);
    }
}
public class SellerCreateRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
}