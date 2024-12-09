using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using System.Security.Claims;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Authorize]
[Route("[controller]")]
public class TransactionController : Controller
{
    public ABCCommerceContext AbcDb { get; }

    public TransactionController(ABCCommerceContext abcDb)
    {
        AbcDb = abcDb;
    }
    [HttpGet()]
    public ActionResult<IEnumerable<Transaction>> GetUserTransactions(int? skip, int? count)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }

        var transactions = AbcDb.Transactions.Where(t => t.UserId == id).Include(t => t.Items).ThenInclude(t => t.Item).ThenInclude(i => i.Seller)
            .OrderByDescending(t => t.PurchaseDate)
            .Skip(skip ?? 0)
            .Take(Math.Min(50, count ?? 50))
            .Select(t => t.ToDto()).ToArray();
        return Ok(transactions);
    }
    [HttpGet("{transactionId:int}")]
    public ActionResult<Transaction> GetUserTransaction(int transactionId)
    {
        if (!int.TryParse(User.FindFirstValue("userid"), out int id))
        {
            return Unauthorized();
        }

        var transaction = AbcDb.Transactions.Where(t => t.UserId == id && t.Id == transactionId).Include(t => t.Items).ThenInclude(t => t.Item).Select(t => t.ToDto()).FirstOrDefault();
        if (transaction is null) return NotFound();
        return Ok(transaction);
    }
}