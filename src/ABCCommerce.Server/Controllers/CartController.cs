
using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using SharedModels.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class CartController : Controller
{

    private readonly ILogger<CartController> _logger;
    public ABCCommerceContext ABCDb { get; }

    public CartController(ILogger<CartController> logger, ABCCommerceContext abcDb)
    {
        _logger = logger;
        ABCDb = abcDb;
    }
    [Authorize]
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
            .ThenInclude(l => l.Item)
            .ThenInclude(i => i.Seller)
            .Select(c => c.ToDto())
            .ToArray();
        return Ok(cartItems);
    }
    [Authorize]
    [HttpDelete("{cartItemId:int}")]
    public ActionResult DeleteCartItem(int cartItemId)
    {
        var cartItem = ABCDb.CartItems.FirstOrDefault(c => c.Id == cartItemId);
        if (cartItem is null) return NotFound();
        ABCDb.CartItems.Remove(cartItem);
        return NoContent();
    }
    [Authorize]
    [HttpPost]
    public ActionResult<CartItem> AddCartItem([FromBody] AddToCartRequest addToCart)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var cartItem = new ABCCommerceDataAccess.Models.CartItem()
        {
            UserId = id,
            ListingId = addToCart.ListingId,
            Quantity = addToCart.Quantity,
            AddDate = DateTime.Now,
        };
        ABCDb.CartItems.Add(cartItem);
        ABCDb.SaveChanges();
        return Ok(cartItem.ToDto());
    }
}
