using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class SellerController : ControllerBase
{

    private readonly ILogger<SellerController> _logger;

    public PermissionService Permission { get; }
    public ABCCommerceContext ABCDb { get; }

    public SellerController(ILogger<SellerController> logger, PermissionService permission, ABCCommerceContext abcDb)
    {
        _logger = logger;
        Permission = permission;
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
    [Authorize]
    [HttpPost(Name = "Add Seller")]
    public async Task<ActionResult<Seller>> AddSeller([FromBody] SellerCreateRequest sellerCreate)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var user = ABCDb.Users.Where(u => u.Id == id).FirstOrDefault();
        if (user is null) return Unauthorized();

        var seller = new ABCCommerceDataAccess.Models.Seller { Name = sellerCreate.Name };
        ABCDb.Sellers.Add(seller);
        await ABCDb.SaveChangesAsync();
        ABCDb.UserSellers.Add(new ABCCommerceDataAccess.Models.UserSeller
        {
            UserId = id,
            SellerId = seller.Id,
            Role = "Owner"
        });
        
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
    [Authorize]
    [HttpGet("{sellerId:int}/Items/{sku}/Exists", Name = "Get Seller Item Exists")]
    public ActionResult<ItemExists> GetItemExists(int sellerId, string sku)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        if (!Permission.IsMember(id, sellerId)) return Unauthorized();
        bool exists = ABCDb.Items.Where(i => i.SellerId == sellerId && i.SKU == sku).Any();
        return Ok(new ItemExists(exists));
    }
    [HttpGet("{sellerId:int}/Members")]
    public ActionResult<IEnumerable<Member>> GetMembers(int sellerId)
    {
        var members = ABCDb.UserSellers.Where(u => u.SellerId == sellerId).Include(u => u.User)
            .Select(u => u.ToDto()).ToArray();
        if (members.Length == 0) return NotFound();
        return Ok(members);
    }
    [Authorize]
    [HttpPatch("{sellerId:int}/Members/{userId:int}/Role")]
    public ActionResult ChangeRole(int sellerId, int userId, [FromBody] MemberRoleChange roleChange)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        if (!Permission.GetPermissionLevel(id, sellerId, out string? modifierPermissionLevel)) return Unauthorized();
        if (!Permission.RoleExists(roleChange.NewRole)) return NotFound("Role not found.");
        if (!Permission.HasExplicitPermission(modifierPermissionLevel, roleChange.NewRole)) return Unauthorized();
        if (id != userId)
        {
            if (!Permission.GetPermissionLevel(userId, sellerId, out string? targetPermissionLevel)) return NotFound("Memeber not found.");
            if (!Permission.HasExplicitPermission(modifierPermissionLevel, targetPermissionLevel)) return Unauthorized();
        }
        Permission.SetPermission(userId, sellerId, roleChange.NewRole);
        return Ok();
    }
    [Authorize]
    [HttpPatch("{sellerId:int}/Members/{userId:int}/Role/Owner")]
    public ActionResult MakeOwner(int sellerId, int userId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        if (!Permission.GetPermissionLevel(id, sellerId, out string? modifierPermissionLevel)) return Unauthorized();
        if (modifierPermissionLevel == PermissionLevel.Personal) return Unauthorized();
        if (!Permission.HasPermission(modifierPermissionLevel, PermissionLevel.Owner)) return Unauthorized();
        if (!Permission.GetPermissionLevel(userId, sellerId, out string? targetPermissionLevel)) return NotFound("Memeber not found.");

        Permission.SetPermission(userId, sellerId, PermissionLevel.Owner);
        return Ok();
    }
    [Authorize]
    [HttpPut("{sellerId:int}/Members/{userId:int}")]
    public ActionResult AddMember(int sellerId, int userId, [FromBody] MemberRoleChange roleChange)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        if (!Permission.GetPermissionLevel(id, sellerId, out string? modifierPermissionLevel)) return Unauthorized();
        if (!Permission.RoleExists(roleChange.NewRole)) return NotFound("Role not found.");
        if (Permission.IsMember(userId, sellerId)) return BadRequest("User is already a member of the seller.");

        if(!Permission.HasExplicitPermission(modifierPermissionLevel, roleChange.NewRole)) return Unauthorized();

        Permission.SetPermission(userId, sellerId, roleChange.NewRole);
        return Ok();
    }
    [Authorize]
    [HttpDelete("{sellerId:int}/Members/{userId:int}")]
    public ActionResult RemoveMember(int sellerId, int userId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        if (!Permission.GetPermissionLevel(id, sellerId, out string? modifierPermissionLevel)) return Unauthorized();
        if(id != userId)
        {
            if (!Permission.GetPermissionLevel(userId, sellerId, out string? targetPermissionLevel)) return BadRequest("User is not a member.");
            if(!Permission.HasExplicitPermission(modifierPermissionLevel, targetPermissionLevel)) return Unauthorized();
        }

        Permission.DeleteMember(userId, sellerId);
        return Ok();
    }
}
public class MemberRoleChange
{
    [Required]
    public string NewRole { get; set; }
}

