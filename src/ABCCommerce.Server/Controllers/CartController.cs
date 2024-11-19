
using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using SharedModels.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class CartController : Controller
{

    private readonly ILogger<CartController> _logger;

    public ListingService ListingService { get; }
    public ABCCommerceContext ABCDb { get; }

    public CartController(ListingService listingService, ILogger<CartController> logger, ABCCommerceContext abcDb)
    {
        ListingService = listingService;
        _logger = logger;
        ABCDb = abcDb;
    }
    [HttpGet]
    public ActionResult<IEnumerable<CartItem>> GetCart()
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var cartItems = ABCDb.CartItems
            .Where(c => c.UserId == id)
            .Include(c => c.Listing)
            .ThenInclude(l => l.Images)
            .Include(c => c.Listing)
            .ThenInclude(l => l.Item)
            .ThenInclude(i => i.Seller)
            .Select(c => c.ToDto())
            .ToArray();
        return Ok(cartItems);
    }
    [HttpGet("{cartItemId:int}")]
    public ActionResult<CartItem> GetCart(int cartItemId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var cartItem = ABCDb.CartItems
            .Where(c => c.UserId == id && c.Id == cartItemId)
            .Include(c => c.Listing)
            .ThenInclude(l => l.Item)
            .ThenInclude(i => i.Seller)
            .Select(c => c.ToDto())
            .FirstOrDefault();
        if (cartItem == null) return NotFound();
        return Ok(cartItem);
    }
    [HttpPatch("{cartItemId:int}")]
    public ActionResult<CartItem> PatchCart(int cartItemId, [FromBody] CartPatchRequest cartPatch)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        if(cartPatch.Quantity is not int newQuantity)
        {
            return ValidationProblem("Body must contain an updated field.", title: "Must contain an updated field");
        }
        var cartItem = ABCDb.CartItems
            .Where(c => c.UserId == id && c.Id == cartItemId)
            .Include(c => c.Listing)
            .ThenInclude(l => l.Item)
            .ThenInclude(i => i.Seller)
            .FirstOrDefault();
        if (cartItem == null) return NotFound();


        cartItem.Quantity = newQuantity;
        cartItem.ModifiedDate = DateTime.Now;
        cartItem.ReservationExpirationDate = null;
        ABCDb.SaveChanges();

        return Ok(cartItem.ToDto());
    }
    [HttpDelete("{cartItemId:int}")]
    public ActionResult DeleteCartItem(int cartItemId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        using var transaction = ABCDb.Database.BeginTransaction();
        var deleteCount = ABCDb.CartItems.Where(c => c.UserId == id && c.Id == cartItemId).ExecuteDelete();
        if (deleteCount > 1)
        {
            transaction.Rollback();
            return Problem("There was an issue removing the cart item.");
        }
        if (deleteCount == 0) return NotFound();

        ABCDb.SaveChanges();
        transaction.Commit();

        return NoContent();
    }
    [HttpPost]
    public ActionResult<CartItem> AddCartItem([FromBody] AddToCartRequest addToCart)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }

        DateTime now = DateTime.Now;
        var cartItem = new ABCCommerceDataAccess.Models.CartItem()
        {
            UserId = id,
            ListingId = addToCart.ListingId,
            Quantity = addToCart.Quantity,
            AddDate = now,
            ModifiedDate = now,
        };
        ABCDb.CartItems.Add(cartItem);
        ABCDb.SaveChanges();
        return Ok(cartItem.ToDto());
    }
    [HttpPost("purchase")]
    public ActionResult PurchaseCartItems([FromBody] PurchaseItemsRequest purchaseItems)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var requestItems = purchaseItems.CartItems.OrderBy(i => i.CartItem).ToList();

        var cartItems = ABCDb.CartItems.Where(c => c.UserId == id && requestItems.Any(i => c.Id == i.CartItem)).OrderBy(c => c.Id).ToArray();

        var errorDetails = ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);


        List<int> notInCartItems = new();
        List<int> tooMuchQuantityItems = new();
        int j = 0;

        for(int i = 0; i < requestItems.Count; i++)
        {
            var cartItem = cartItems[j];
            var requestItem = requestItems[i];
            if(cartItem.Id != requestItem.CartItem)
            {
                notInCartItems.Add(requestItem.CartItem);
            }
            else
            {
                if(requestItem.Quantity > cartItem.Quantity)
                {
                    tooMuchQuantityItems.Add(requestItem.CartItem);
                }
                else
                {
                    //ListingService.IsCountAvailable(requestItem.Quantity)
                }
                j++;
            }
        }
        if(notInCartItems.Count > 0)
        {
            errorDetails.Errors["Item not in cart."] = notInCartItems.Select(i => $"{i} is not a cart id.").ToArray();
        }
        if(tooMuchQuantityItems.Count > 0)
        {
            errorDetails.Errors["Quantity greater than expected."] = tooMuchQuantityItems.Select(i => $"{i} is to many items. Add more quantity to cart.").ToArray();
        }
        if(errorDetails.Errors.Count != 0)
        {
            return ValidationProblem(errorDetails);
        }

        var listings = ABCDb.Listings.Where(l => cartItems.Any(c => c.ListingId == l.Id)).OrderBy(l => l.Id);
        throw new NotImplementedException();
    }
}
