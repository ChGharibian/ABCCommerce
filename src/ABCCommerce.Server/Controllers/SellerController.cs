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
        return Ok(ABCDb.Sellers.Select(s => new BasicSeller(s.Id, s.Image, s.Name)).ToArray());
    }
    [HttpGet("{id:int}", Name = "GetSeller")]
    public async Task<ActionResult<Seller>> GetSeller(int id)
    {
        var seller = await ABCDb.Sellers
            .Where(s => s.Id == id)
            .Include(s => s.Items)
            .ThenInclude(i => i.Listings.Where(l => l.Active))
            .FirstOrDefaultAsync();
        if(seller is null)
        {
            return NotFound();
        }
        return Ok(new ComplexSeller(
            seller.Name,
            seller.Image,
            seller.Id,
seller.Items.SelectMany(i => i.Listings.Select(l => l.ToDto()))
        ));
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

internal class BasicSeller
{
    public int Id { get; }
    [Image]
    public string? Image { get; set; }
    public string Name { get; }

    public BasicSeller(int id, string? image, string name)
    {
        Id = id;
        Image = image;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        return obj is BasicSeller other &&
               Id == other.Id &&
               Image == other.Image &&
               Name == other.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Image, Name);
    }
}

internal class ComplexSeller
{
    public string Name { get; }
    [Image]
    public string? Image { get; set; }
    public int Id { get; }
    public IEnumerable<ListingDTO> Items { get; }

    public ComplexSeller(string name, string? image, int id, IEnumerable<ListingDTO> items)
    {
        Name = name;
        Image = image;
        Id = id;
        Items = items;
    }

    public override bool Equals(object? obj)
    {
        return obj is ComplexSeller other &&
               Name == other.Name &&
               Image == other.Image &&
               Id == other.Id &&
               EqualityComparer<IEnumerable<ListingDTO>>.Default.Equals(Items, other.Items);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Image, Id, Items);
    }
}