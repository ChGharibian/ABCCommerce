using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.AspNetCore.StaticFiles;
using System.Text;
using Microsoft.Extensions.Hosting;
using SharedModels.Models;
using Microsoft.EntityFrameworkCore;
namespace ABCCommerce.Server.Services;

public class ListingService
{
    public ABCCommerceContext AbcDb { get; }
    public ListingService(ABCCommerceContext abcDb)
    {
        AbcDb = abcDb;
    }
    public async Task<ListingAvailability?> Availability(int id, IEnumerable<int> excludeCartItems)
    {
        var quantity = AbcDb.Listings.Where(l => l.Id == id && l.Active).Select(l => new { l.Quantity }).FirstOrDefault()?.Quantity ?? -1;
        if (quantity == -1) return null;

        var requested = await AbcDb.CartItems.Where(i => i.ListingId == id && !excludeCartItems.Any(c => c == i.Id)).SumAsync(i => i.Quantity);
        quantity -= requested;
        DateTime availabilityDate = DateTime.Now;
        if (quantity <= 0)
        {
            var overdraw = -quantity;
            ABCCommerceDataAccess.Models.CartItem[] fromDbCartItems = await AbcDb.CartItems.Where(i => i.ListingId == id).OrderBy(i => i.AddDate).Take(overdraw + 1).ToArrayAsync();
            var openDate = fromDbCartItems.SkipWhile(i => (overdraw -= i.Quantity) > 0).Select(i => i.AddDate).First();
            availabilityDate = openDate.AddDays(5);
        }

        return new ListingAvailability(Math.Max(quantity, 0), availabilityDate);
    }
}

public class ListingAvailability
{
    public int Quantity { get; }
    public DateTime Avaliability { get; }

    public ListingAvailability(int quantity, DateTime avaliability)
    {
        Quantity = quantity;
        Avaliability = avaliability;
    }

    public override bool Equals(object? obj)
    {
        return obj is ListingAvailability other &&
               Quantity == other.Quantity &&
               Avaliability == other.Avaliability;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Quantity, Avaliability);
    }
}