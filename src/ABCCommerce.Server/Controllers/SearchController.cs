
using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Examine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

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
    public ActionResult Search([FromQuery] string q)
    {
        var searchResults = ExamineManager.GetIndex("MyIndex").Searcher.CreateQuery()
            .ManagedQuery(q).Execute();

        var ids = searchResults.Select(s => int.Parse(s.Id)).ToList();

        return Ok(AbcDb.Listings
            .Where(l => ids.Contains(l.Id))
            .Include(l => l.Item)
            .ThenInclude(i => i.Seller)
            .Include(l => l.Images)
            .Select(l => l.ToDto()));
    }
}