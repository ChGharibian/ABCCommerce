
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
    public TransactionService TransactionService { get; }
    public ABCCommerceContext ABCDb { get; }

    public CartController(ListingService listingService, TransactionService transactionService, ILogger<CartController> logger, ABCCommerceContext abcDb)
    {
        ListingService = listingService;
        TransactionService = transactionService;
        _logger = logger;
        ABCDb = abcDb;
    }
    /// <summary>
    /// Gets all cart items belonging to the user.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name="Get Cart Items")]
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
    /// <summary>
    /// Get a singular cart item belonging to the user.
    /// </summary>
    /// <param name="cartItemId">The id of the cart item to get.</param>
    /// <response code="404">Could not find cart item belonding to the user.</response>
    [HttpGet("{cartItemId:int}", Name ="Get Cart Item")]
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
    /// <summary>
    /// Updates the users cart item with the provided information. This will reset the users availability of the listing.
    /// </summary>
    /// <param name="cartItemId">The cart item to update.</param>
    /// <param name="cartPatch"></param>
    /// <returns></returns>
    [HttpPatch("{cartItemId:int}", Name = "Update Cart Item")]
    public async Task<ActionResult<CartItem>> PatchCart(int cartItemId, [FromBody] CartPatchRequest cartPatch)
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

        await ListingService.Availability(cartItem.ListingId);

        return Ok(cartItem.ToDto());
    }
    /// <summary>
    /// Deletes the user's cart item.
    /// </summary>
    /// <param name="cartItemId">The cart item of the user to delete.</param>
    /// <returns></returns>
    [HttpDelete("{cartItemId:int}")]
    public async Task<ActionResult> DeleteCartItem(int cartItemId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        using var transaction = ABCDb.Database.BeginTransaction();
        var listingId = ABCDb.CartItems.Where(c => c.UserId == id && c.Id == cartItemId).Select(c => c.ListingId).FirstOrDefault();
        var deleteCount = ABCDb.CartItems.Where(c => c.UserId == id && c.Id == cartItemId).ExecuteDelete();
        if (deleteCount > 1)
        {
            transaction.Rollback();
            return Problem("There was an issue removing the cart item.");
        }
        if (deleteCount == 0) return NotFound();

        ABCDb.SaveChanges();
        transaction.Commit();

        await ListingService.Availability(listingId);

        return NoContent();
    }
    /// <summary>
    /// Add an item to the users cart.
    /// </summary>
    /// <param name="addToCart"></param>
    /// <returns></returns>
    [HttpPost(Name = "Add Cart Item")]
    public async Task<ActionResult<CartItem>> AddCartItem([FromBody] AddToCartRequest addToCart)
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
        await ListingService.Availability(cartItem.Id);
        return Ok(cartItem.ToDto());
    }
    /// <summary>
    /// Purchases the user's provided cart items at the requested quantities.
    /// </summary>
    /// <param name="purchaseItems"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("purchase")]
    public async Task<ActionResult<Transaction>> PurchaseCartItems([FromBody] PurchaseItemsRequest purchaseItems)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var requestItems = purchaseItems.CartItems.OrderBy(i => i.CartItem).ToList();

        var errorDetails = ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);


        List<int> notInCartItems = new();
        List<int> tooMuchQuantityItems = new();
        List<int> notAvailable = new();
        int j = 0;

        for (int i = 0; i < requestItems.Count; i++)
        {
            var requestItem = requestItems[i];
            var cartItem = ABCDb.CartItems.Where(c => requestItem.CartItem == c.Id).FirstOrDefault();
            if (cartItem is null)
            {
                notInCartItems.Add(requestItem.CartItem);
            }
            else
            {
                if (requestItem.Quantity > cartItem.Quantity)
                {
                    tooMuchQuantityItems.Add(requestItem.CartItem);
                }
                else
                {
                    if (!await ListingService.IsCountAvailable(requestItem.Quantity, cartItem.Id))
                    {
                        notAvailable.Add(cartItem.Id);
                    }
                }
                j++;
            }
        }
        if (notInCartItems.Count > 0)
        {
            errorDetails.Errors["Item not in cart."] = notInCartItems.Select(i => i.ToString()).ToArray();
        }
        if (tooMuchQuantityItems.Count > 0)
        {
            errorDetails.Errors["Quantity greater than expected. Add more quantity to cart."] = tooMuchQuantityItems.Select(i => i.ToString()).ToArray();
        }
        if (notAvailable.Count > 0)
        {
            errorDetails.Errors["Quantity not available to purchase."] = notAvailable.Select(i => i.ToString()).ToArray();
        }
        if (errorDetails.Errors.Count != 0)
        {
            return ValidationProblem(errorDetails);
        }
        return Ok(TransactionService.Purchase(purchaseItems, id));
    }
}
