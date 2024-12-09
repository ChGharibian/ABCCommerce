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
[Authorize]
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
    [AllowAnonymous]
    [HttpGet("{userId:int}")]
    public ActionResult<User> GetUser(int userId)
    {
        var user = ABCDb.Users.Where(u => u.Id == userId).FirstOrDefault();
        if(user is null) return NotFound();

        if (this.User.Identity is not null && this.User.Identity.IsAuthenticated && int.TryParse(User.FindFirstValue("userid"), out int id) && id == userId)
        {
            return Ok(user.ToFullDto());
        }
        else
        {
            return Ok(user.ToDto());
        }
    }
    [HttpPatch]
    public ActionResult<User> PatchUser(UserPatchRequest patchRequest)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }
        var user = ABCDb.Users.Where(u => u.Id == id).FirstOrDefault();
        if (user is null) return NotFound("How did you manage to get here?");
        if(patchRequest.Username is null 
            && patchRequest.Street is null 
            && patchRequest.StreetPlus is null
            && patchRequest.City is null
            && patchRequest.State is null
            && patchRequest.Zip is null
            && patchRequest.ContactName is null
            && patchRequest.Phone is null)
        {
            return BadRequest("Must include a change.");
        }
        if (patchRequest.Username is not null) user.Username = patchRequest.Username;
        if (patchRequest.Street is not null) user.Street = patchRequest.Street;
        if (patchRequest.StreetPlus is not null) user.StreetPlus = patchRequest.StreetPlus;
        if (patchRequest.City is not null) user.City = patchRequest.City;
        if (patchRequest.State is not null) user.State = patchRequest.State;
        if (patchRequest.Zip is not null) user.Zip = patchRequest.Zip;
        if (patchRequest.ContactName is not null) user.ContactName = patchRequest.ContactName;
        if (patchRequest.Phone is not null) user.Phone = patchRequest.Phone;
        ABCDb.SaveChanges();
        return Ok(user);
    }
    /// <summary>
    /// Get the cart items belonging to the user.
    /// </summary>
    /// <returns></returns>
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
    [AllowAnonymous]
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
    [AllowAnonymous]
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
            Role = PermissionLevel.Personal,
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
    [AllowAnonymous]
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
