using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.Requests;
using SharedModels.Models.Response;
using System.Security.Claims;

using UserModel = ABCCommerceDataAccess.Models.User;
using SellerModel = ABCCommerceDataAccess.Models.Seller;
using UserSellerModel = ABCCommerceDataAccess.Models.UserSeller;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : Controller
{

    private readonly ILogger<SellerController> _logger;
    public IPasswordHasher<object> PasswordHasher { get; }
    public ABCCommerceContext ABCDb { get; }
    public TokenService TokenService { get; }

    public UserController(ILogger<SellerController> logger, ABCCommerceContext abcDb, TokenService tokenService, IPasswordHasher<object> passwordHasher)
    {
        _logger = logger;
        ABCDb = abcDb;
        TokenService = tokenService;
        PasswordHasher = passwordHasher;
    }
    /// <summary>
    /// Get the cart items belonging to the user.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("Cart", Name = "Get User Cart Items")]
    public ActionResult GetCart()
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
    /// <summary>
    /// Get the sellers the user belongs to.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("Sellers", Name = "Get User Sellers")]
    public ActionResult<IEnumerable<Seller>> GetSellers()
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var sellers = ABCDb.UserSellers
            .Where(c => c.UserId == id)
            .Include(c => c.Seller)
            .Select(c => c.Seller.ToDto())
            .ToArray();
        return Ok(sellers);
    }
    /// <summary>
    /// Refresh a user login token.
    /// </summary>
    /// <param name="refreshRequest"></param>
    /// <returns></returns>
    [HttpPost("Refresh", Name = "Refresh User Token")]
    public ActionResult<TokenResponse> RefreshUserToken([FromBody] RefreshTokenRequest refreshRequest)
    {
        var claims = TokenService.ValidateToken(refreshRequest.RefreshToken, out var token);
        var userid = int.Parse(claims.FindFirstValue("userid")!);
        var user = ABCDb.Users.FirstOrDefault(u => u.Id == userid);
        if (user is null) return NotFound();
        return TokenService.CreateToken(user);
    }
    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="registerUserRequest"></param>
    /// <returns></returns>
    [HttpPost("Register", Name = "Register New User")]
    public ActionResult<TokenResponse> RegisterUser([FromBody] RegisterUserRequest registerUserRequest)
    {
        if (ABCDb.Users.Any(u => u.Email == registerUserRequest.Email))
        {
            return BadRequest("Email is already taken.");
        }
        var user = new UserModel
        {
            Email = registerUserRequest.Email,
            Username = registerUserRequest.Username ?? "",
            Password = PasswordHasher.HashPassword(null, registerUserRequest.Password),
            Street = registerUserRequest.Street,
            StreetPlus = registerUserRequest.StreetPlus,
            City = registerUserRequest.City,
            State = registerUserRequest.State,
            Zip = registerUserRequest.Zip,
            Roles = "User",
        };

        ABCDb.Users.Add(user);
        ABCDb.SaveChanges();

        var seller = new SellerModel()
        {
            Name = registerUserRequest.Email,
        };

        ABCDb.Sellers.Add(seller);
        ABCDb.SaveChanges();

        var userSeler = new UserSellerModel()
        {
            User = user,
            Seller = seller,
            Role = "Owner",
        };

        ABCDb.UserSellers.Add(userSeler);
        ABCDb.SaveChanges();

        return Ok(TokenService.CreateToken(user));
    }
    /// <summary>
    /// Login to an existing user.
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    [HttpPost("Login", Name = "Login To User")]
    public ActionResult<TokenResponse> LoginUser([FromBody] LoginRequest loginRequest)
    {
        var user = ABCDb.Users
            .Where(u => u.Email == loginRequest.Username)
            .FirstOrDefault();
        if (user is null) return NotFound();
        var verifyResult = PasswordHasher.VerifyHashedPassword(null, user.Password, loginRequest.Password);

        switch (verifyResult)
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
                user.Password = PasswordHasher.HashPassword(null, user.Password);
                ABCDb.SaveChanges();
                break;
            case PasswordVerificationResult.Success:
                break;
            default:
                return NotFound();
        }

        return Ok(TokenService.CreateToken(user));
    }
}
