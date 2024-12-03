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
    /// <summary>
    /// Find out if <paramref name="count"/> number of items from <paramref name="cartItemId"/> is available.
    /// </summary>
    /// <param name="count">The number of items that need to be available. Must be less than or equal to the cart item's quantity defined by <paramref name="cartItemId"/></param>
    /// <param name="cartItemId">The id of the cart item to check availability for.</param>
    /// <returns></returns>
    public async Task<bool> IsCountAvailable(int count, int cartItemId)
    {
        DateTime now = DateTime.Now;
        var cartItem = AbcDb.CartItems.Where(l => l.Id == cartItemId).FirstOrDefault();
        if(cartItem is null || cartItem.Quantity < count)
        {
            return false;
        }
        bool wasAlreadyCheckedForReserved = false;
        if(cartItem.ReservationExpirationDate is DateTime t1 && t1 > now)
        {
            count -= cartItem.Quantity;
            wasAlreadyCheckedForReserved = true;
        }
        if (count <= 0) return true;
        count -= await Availability(cartItem.ListingId);
        if (count <= 0) return true;

        if (!wasAlreadyCheckedForReserved && cartItem.ReservationExpirationDate is DateTime t2 && t2 > now)
        {
            count -= cartItem.Quantity;
        }
        return count <= 0;
    }
    /// <summary>
    /// Determine the current quantity of unreserved items in the listing.
    /// </summary>
    /// <param name="listingId">The id of the listing to check availability for.</param>
    /// <returns></returns>
    public async Task<int> Availability(int listingId)
    {
        var totalQuantity = (await AbcDb.Listings.Where(l => l.Id == listingId && l.Active).Select(l => new { l.Quantity }).FirstOrDefaultAsync())?.Quantity ?? -1;
        if (totalQuantity == -1) return 0;
        
        DateTime now = DateTime.Now;
        int reserved = await AbcDb.CartItems.Where(l => l.ListingId == listingId && l.ReservationExpirationDate != null && l.ReservationExpirationDate > now).SumAsync(i => i.Quantity);

        int availableQuantity = totalQuantity - reserved;
        while (availableQuantity > 0)
        {
            var queuedItem = await AbcDb.CartItems.Where(l => l.ListingId == listingId && l.ReservationExpirationDate == null).OrderBy(l => l.AddDate).FirstOrDefaultAsync();
            if (queuedItem is null) break;
            if (availableQuantity >= queuedItem.Quantity)
            {
                queuedItem.ReservationExpirationDate = now.AddDays(5);
                availableQuantity -= queuedItem.Quantity;
                await AbcDb.SaveChangesAsync();
            }
            else
            {
                availableQuantity = 0;
            }
        }
        return availableQuantity;
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