using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models.Requests;

namespace ABCCommerce.Server.Services;

public class TransactionService
{
    public ABCCommerceContext AbcDb { get; }
    public TransactionService(ABCCommerceContext abcDb)
    {
        AbcDb = abcDb;
    }
    public Transaction Purchase(PurchaseItemsRequest itemsRequest, int userId)
    {
        var dbTransaction = AbcDb.Database.BeginTransaction();
        try
        {
            var transaction = new Transaction()
            {
                Last4 = itemsRequest.CardNumber[(Math.Max(0, itemsRequest.CardNumber.Length - 4))..],
                UserId = userId,
                TotalPrice = 0,

            };
            var requestItems = itemsRequest.CartItems.ToArray();
            for (int i = 0; i < requestItems.Length; i++)
            {
                var requestItem = requestItems[i];
                var cartItem = AbcDb.CartItems.Where(c => requestItem.CartItem == c.Id).Include(c => c.Listing).FirstOrDefault();
                if (cartItem is null) throw new BadHttpRequestException("This should not have occured.");
                decimal price = decimal.Round(requestItem.Quantity * cartItem.Listing.PricePerUnit, 2);
                var transactionItem = new TransactionItem()
                {
                    ItemId = cartItem.Listing.ItemId,
                    Quantity = requestItem.Quantity,
                    UnitPrice = cartItem.Listing.PricePerUnit,
                    TotalPrice = price,
                };
                cartItem.Listing.Quantity -= requestItem.Quantity;
                if (cartItem.Listing.Quantity < 0)
                {
                    throw new BadHttpRequestException("Quantity was greater than the available amount. This should not have occured.");
                }
                cartItem.Quantity -= requestItem.Quantity;
                if(cartItem.Quantity == 0)
                {
                    AbcDb.CartItems.Remove(cartItem);
                } else if(cartItem.Quantity < 0)
                {
                    throw new BadHttpRequestException("Quantity was greater than the maximum amount. This should not have occured.");
                }
                transaction.Items.Add(transactionItem);
                transaction.TotalPrice = decimal.Round(price + transaction.TotalPrice, 2);
            }
            transaction.PurchaseDate = DateTime.Now;
            AbcDb.SaveChanges();
            dbTransaction.Commit();
            return transaction;
        }
        catch
        {
            dbTransaction.Rollback();
            throw;
        }
    }
}