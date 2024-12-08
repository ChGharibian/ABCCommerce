using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Examine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ABCCommerce.Server.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class SellerController : ControllerBase
{

    private readonly ILogger<SellerController> _logger;

    public PermissionService Permission { get; }
    public ABCCommerceContext ABCDb { get; }
    public IExamineManager ExamineManager { get; }
    public IImageService ImageService { get; }

    public SellerController(ILogger<SellerController> logger, PermissionService permission, IExamineManager examineManager, ABCCommerceContext abcDb, IImageService imageService)
    {
        _logger = logger;
        Permission = permission;
        ExamineManager = examineManager;
        ABCDb = abcDb;
        ImageService = imageService;
    }

    /// <summary>
    /// Get all sellers.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "Get Sellers")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<Seller>> Get(int? skip, int? count)
    {
        return Ok(ABCDb.Sellers.OrderBy(s => s.Id).Skip(skip ?? 0).Take(Math.Min(50, count ?? 50)).Select(s => s.ToDto()).ToArray());
    }
    /// <summary>
    /// Request a seller using the provided seller id.
    /// </summary>
    /// <param name="sellerId">The id of the requested seller.</param>
    /// <returns></returns>
    [HttpGet("{sellerId:int}", Name = "Get Seller")]
    [AllowAnonymous]
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
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }
        
        Listing[] listings = await ABCDb.Listings
                    .Include(l => l.Item)
                    .Include(l => l.Images)
                    .Where(l => l.Item.SellerId == sellerId)
                    .Skip(skip ?? 0)
                    .Take(Math.Min(50, count ?? 50))
                    .Select(l => l.ToDto())
                    .ToArrayAsync();
        return base.Ok(listings);
    }

    /// <summary>
    /// Creates a listing.
    /// </summary>
    /// <param name="sellerId"></param>
    /// <param name="createListing"></param>
    [HttpPost("{sellerId:int}/Listings", Name = "Create Listing")]
    public async Task<ActionResult<Listing>> CreateListing(int sellerId, [FromBody] CreateListingRequest createListing)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var item = await ABCDb.Items.Include(i => i.Listings).Include(i => i.Seller).Where(i => i.Id == createListing.Item).FirstOrDefaultAsync();
        if (item is null)
        {
            return NotFound();
        }
        var listing = new ABCCommerceDataAccess.Models.Listing
        {
            Name = createListing.Name,
            Active = createListing.Active,
            ListingDate = DateTime.UtcNow,
            Description = createListing.Description,
            PricePerUnit = createListing.Price,
            Quantity = createListing.Quantity,
            Tags = createListing.Tags.ToArray(),
        };
        item.Listings.Add(listing);
        await ABCDb.SaveChangesAsync();
        ExamineManager.GetIndex("MyIndex").Index(listing);
        return Ok(listing.ToDto());
    }
    /// <summary>
    /// Updates a listing with the provided information.
    /// </summary>
    /// <param name="sellerId">The id of the seller to be updated</param>
    /// <param name="listingId">The id of the listing to be updated</param>
    /// <param name="updateRequest"></param>
    /// <response code="200">Successfully updated the listing.</response>
    /// <response code="400">Some information provided in the body was incorrect.</response>
    /// <response code="404">Could not find the listing.</response>
    [HttpPatch("{sellerId:int}/Listings/{listingId:int}", Name = "Update Listing")]
    public async Task<ActionResult<Listing>> UpdateListing(int sellerId, int listingId, [FromBody] UpdateListingRequest updateRequest)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var editListing = await ABCDb.Listings.Where(l => l.Id == listingId && l.Item.SellerId == sellerId).Include(l => l.Images).FirstOrDefaultAsync();
        if (editListing is null) return NotFound();
        if (updateRequest.Active is bool b)
        {
            editListing.Active = b;
        }
        if (updateRequest.Price is decimal d)
        {
            editListing.PricePerUnit = d;
        }
        if (updateRequest.Description is string s)
        {
            editListing.Description = s;
        }
        if (updateRequest.Quantity is int i)
        {
            editListing.Quantity = i;
        }
        if (updateRequest.RemoveTags is not null || updateRequest.AddTags is not null)
        {
            editListing.Tags = editListing.Tags.Except(updateRequest.RemoveTags ?? Array.Empty<string>()).Concat(updateRequest.AddTags ?? Array.Empty<string>()).ToArray();
        }
        ABCDb.SaveChanges();
        return editListing.ToDto();
    }

    /// <summary>
    /// Adds image to a listing.
    /// </summary>
    /// <param name="listingId">The id of the listing to add an image to.</param>
    /// <param name="imageRequest"></param>
    /// <returns></returns>
    [HttpPost("{sellerId:int}/Listings/{listingId:int}/Image", Name = "Add Image To Listing")]
    public ActionResult<ImagePath> AddImageToListing(int sellerId, int listingId, [FromBody] AddImageRequest imageRequest)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }
        var editListing = ABCDb.Listings.Where(l => l.Item.SellerId == sellerId && l.Id == listingId).Include(l => l.Images).FirstOrDefault();
        if (editListing is null)
            return NotFound();

        var imagePath = ImageService.AddImage(imageRequest.Image, imageRequest.FileType, "listings");

        editListing.Images.Add(new ABCCommerceDataAccess.Models.ListingImage() { Image = imagePath.Path });
        ABCDb.SaveChanges();
        return Ok(imagePath);
    }
    /// <summary>
    /// Delete an image from a listing.
    /// </summary>
    /// <param name="listingId">The id of the listing to delete the image from.</param>
    /// <param name="imageId">The listing image id of the liting image to delete.</param>
    /// <returns></returns>
    [HttpDelete("{sellerId:int}/Listings/{listingId:int}/Image/{imageId:int}", Name = "Remove Image From Listing")]
    public ActionResult DeleteImageInListing(int sellerId, int listingId, int imageId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }
        var imageToDelete = ABCDb.ListingImages.Where(li => li.Listing.Item.SellerId == sellerId && li.ListingId == listingId && li.Id == imageId).FirstOrDefault();

        if (imageToDelete is null) return NotFound();

        ImageService.DeleteImage(imageToDelete.Image);
        ABCDb.SaveChanges();
        return Ok();
    }
    /// <summary>
    /// Get the items belonging to the seller.
    /// </summary>
    /// <param name="sellerId">The id of the seller the items belong to.</param>
    /// <returns></returns>
    [HttpGet("{sellerId:int}/Items", Name = "Get Seller Items")]
    public ActionResult<IEnumerable<Item>> GetItems(int sellerId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        return Ok(ABCDb.Items.Where(i => i.SellerId == sellerId).Include(i => i.Listings).Select(i => i.ToDto()).ToArray());
    }

    /// <summary>
    /// Used to query seller items based on the sku.
    /// </summary>
    /// <param name="sellerId">The seller the item belongs to.</param>
    /// <param name="sku">The sku of the item.</param>
    /// <repsonse code="200">Returned item.</repsonse>
    /// <repsonse code="404">Item sku does not belond the the seller.</repsonse>
    [HttpGet("{sellerId:int}/Items/SKU/{sku}", Name = "Get Seller Item")]
    public ActionResult<IEnumerable<Item>> GetItem(int sellerId, string sku)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var item = ABCDb.Items.Where(i => i.SellerId == sellerId && i.SKU == sku).Include(i => i.Listings).Select(i => i.ToDto()).FirstOrDefault();
        if (item is null) return NotFound();
        return Ok(item);
    }

    /// <summary>
    /// Created an item.
    /// </summary>
    /// <param name="createItem"></param>
    /// <returns></returns>
    [HttpPost("{sellerId:int}/Item", Name = "Create Item")]
    public async Task<ActionResult<Item>> CreateItem(int sellerId, [FromBody] CreateItemRequest createItem)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var seller = await ABCDb.Sellers.FirstOrDefaultAsync(s => s.Id == sellerId);
        if (seller is null)
        {
            return BadRequest(new
            {
                Error = "Seller Not Found"
            });
        }
        if (ABCDb.Items.Any(i => i.SellerId == seller.Id && i.SKU == createItem.Sku))
        {
            return Problem("Item sku already exists.", statusCode: StatusCodes.Status400BadRequest);
        }
        var item = new ABCCommerceDataAccess.Models.Item
        {
            SKU = createItem.Sku,
            Name = createItem.Name,
        };
        seller.Items.Add(item);
        await ABCDb.SaveChangesAsync();
        return Ok(item.ToDto());
    }
    [HttpGet("{sellerId:int}/Items/{itemId:int}/CanModify", Name = "Can Modify Item")]
    public ActionResult<bool> CanModifyItem(int sellerId, int itemId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var item = ABCDb.Items.Where(i => i.SellerId == sellerId && i.Id == itemId).Include(i => i.Listings).Select(i => i.ToDto()).FirstOrDefault();
        if (item is null) return NotFound();
        if (ABCDb.Listings.Any(t => t.ItemId == item.Id))
        {
            return Ok(false);
        }
        if (ABCDb.TransactionItems.Any(t => t.ItemId == item.Id))
        {
            return Ok(false);
        }
        return Ok(true);
    }
    [HttpPatch("{sellerId:int}/Items/{itemId:int}", Name = "Patch Item")]
    public ActionResult<Item> PatchItem(int sellerId, int itemId, [FromBody] ItemPatchRequest itemPatch)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var item = ABCDb.Items.Where(i => i.SellerId == sellerId && i.Id == itemId).Include(i => i.Listings).Select(i => i.ToDto()).FirstOrDefault();
        if (item is null) return NotFound();
        if (ABCDb.Listings.Any(t => t.ItemId == item.Id))
        {
            return Unauthorized("Cannot modify item with listings.");
        }
        if (ABCDb.TransactionItems.Any(t => t.ItemId == item.Id))
        {
            return Unauthorized("Connot modify item with transactins.");
        }
        if(itemPatch.Sku == null && itemPatch.Name == null)
        {
            return Problem("At least one thing must be modified.", statusCode: StatusCodes.Status400BadRequest);
        }
        if(itemPatch.Sku is not null)
        {
            if (ABCDb.Items.Any(i => i.SellerId == sellerId && i.SKU == itemPatch.Sku))
            {
                return Problem("Item sku already exists.", statusCode: StatusCodes.Status400BadRequest);
            }
            item.Sku = itemPatch.Sku;
        }
        if(itemPatch.Name is not null)
        {
            item.Name = itemPatch.Name;
        }
        ABCDb.SaveChanges();
        return Ok(item);
    }
    [HttpDelete("{sellerId:int}/Items/{itemId:int}", Name = "Delete Seller Item")]
    public ActionResult DeleteItem(int sellerId, int itemId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        var item = ABCDb.Items.Where(i => i.SellerId == sellerId && i.Id == itemId).Include(i => i.Listings).FirstOrDefault();
        if (item is null) return NotFound();
        if(ABCDb.Listings.Any(t => t.ItemId == item.Id))
        {
            return Unauthorized("Cannot remove item with listings.");
        }
        if(ABCDb.TransactionItems.Any(t => t.ItemId == item.Id))
        {
            return Unauthorized("Cannot remove item with transactions.");
        }
        ABCDb.Items.Remove(item);
        ABCDb.SaveChanges();
        return Ok();
    }
    /// <summary>
    /// Check if the item sku is already being used by the seller.
    /// </summary>
    /// <param name="sellerId">The seller the item belongs to.</param>
    /// <param name="sku">The sku of the item.</param>
    [Authorize]
    [HttpGet("{sellerId:int}/Items/SKU/{sku}/Exists", Name = "Get Seller Item Exists")]
    public ActionResult<ItemExists> GetItemExists(int sellerId, string sku)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id) || !Permission.IsMember(id, sellerId))
        {
            return Unauthorized();
        }

        if (!Permission.IsMember(id, sellerId)) return Unauthorized();
        bool exists = ABCDb.Items.Where(i => i.SellerId == sellerId && i.SKU == sku).Any();
        return Ok(new ItemExists(exists));
    }
    [AllowAnonymous]
    [HttpGet("{sellerId:int}/Members")]
    public ActionResult<IEnumerable<Member>> GetMembers(int sellerId)
    {
        var members = ABCDb.UserSellers.Where(u => u.SellerId == sellerId).Include(u => u.User)
            .Select(u => u.ToDto()).ToArray();
        if (members.Length == 0) return NotFound();
        return Ok(members);
    }
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
            if (!Permission.HasExplicitPermission(modifierPermissionLevel, targetPermissionLevel)) return Unauthorized();
        }
        else
        {
            if (modifierPermissionLevel == PermissionLevel.Personal) return Unauthorized();
            if (modifierPermissionLevel == PermissionLevel.Owner)
            {
                if (ABCDb.UserSellers.Count(u => u.SellerId == sellerId && (u.Role == PermissionLevel.Owner || u.Role == PermissionLevel.Personal)) < 2) return Unauthorized();
            }
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


public class ItemPatchRequest
{
    [MinLength(1)]
    public string? Sku { get; set; }
    [MinLength(1)]
    public string? Name { get; set; }
}