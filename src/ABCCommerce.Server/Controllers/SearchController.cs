using ABCCommerceDataAccess;
using Examine;
using Examine.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace ABCCommerce.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class SearchController : Controller
{
    ABCCommerceContext AbcDb { get; }
    public IExamineManager ExamineManager { get; }

    public SearchController(ABCCommerceContext abcDb, IExamineManager examineManager)
    {
        AbcDb = abcDb;
        ExamineManager = examineManager;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Listing>> Search([FromQuery] string q, [FromQuery] int? skip, [FromQuery] int? count)
    {
        var searchResults = ExamineManager.GetIndex("MyIndex").Searcher.CreateQuery()
            .ManagedQuery(q).Execute(QueryOptions.SkipTake(skip ?? 0, Math.Min(50, count ?? 50)));

        var ids = searchResults.Select(s => int.Parse(s.Id)).ToList();

        return Ok(AbcDb.Listings
            .Where(l => ids.Contains(l.Id))
            .Include(l => l.Item)
            .ThenInclude(i => i.Seller)
            .Include(l => l.Images)
            .Select(l => l.ToDto()));
    }
}