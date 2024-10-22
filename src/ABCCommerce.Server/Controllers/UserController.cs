
using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models.Requests;
using SharedModels.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
    [HttpPost("Refresh")]
    public ActionResult<TokenResponse> RefreshUserToken([FromBody] RefreshTokenRequest refreshRequest)
    {
        var claims = TokenService.ValidateToken(refreshRequest.RefreshToken, out var token);
        var userid = int.Parse(claims.FindFirstValue("userid")!);
        var user = ABCDb.Users.FirstOrDefault(u => u.Id == userid);
        if (user is null) return NotFound();
        return TokenService.CreateToken(user);
    }
    [HttpPost("Register")]
    public ActionResult<TokenResponse> RegisterUser([FromBody] RegisterUserRequest registerUserRequest)
    {
        if(ABCDb.Users.Any(u => u.Email == registerUserRequest.Email))
        {
            return BadRequest("Email is already taken.");
        }
        var user = new User
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

        var seller = new Seller()
        {
            Name = registerUserRequest.Email,
        };

        ABCDb.Sellers.Add(seller);
        ABCDb.SaveChanges();

        var userSeler = new UserSeller()
        {
            User = user,
            Seller = seller,
            Role = "Owner",
        };

        ABCDb.UserSellers.Add(userSeler);
        ABCDb.SaveChanges();

        return Ok(TokenService.CreateToken(user));
    }
    [HttpPost("Login")]
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
