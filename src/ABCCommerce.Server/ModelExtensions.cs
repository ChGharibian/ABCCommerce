using System.Runtime.InteropServices;
using ListingDTO = SharedModels.Models.Listing;
using ItemDTO = SharedModels.Models.Item;
using SellerDTO = SharedModels.Models.Seller;
using CartItemDTO = SharedModels.Models.CartItem;
using MemberDTO = SharedModels.Models.Member;
using UserDTO = SharedModels.Models.User;
using TransactionDTO = SharedModels.Models.Transaction;
using TransactionItemDTO = SharedModels.Models.TransactionItem;
using ABCCommerceDataAccess.Models;

namespace ABCCommerce.Server;

public static class ModelExtensions
{
    public static ListingDTO ToDto(this Listing listing)
    {
        return new ListingDTO
        {
            Id = listing.Id,
            Name = listing.Name,
            Active = listing.Active,
            Description = listing.Description,
            ListingDate = listing.ListingDate,
            PricePerUnit = listing.PricePerUnit,
            Quantity = listing.Quantity,
            Tags = listing.Tags,
            ImageIds = listing.Images.OrderBy(i => i.Id).Select(i => i.Id).ToArray(),
            Images = listing.Images.OrderBy(i => i.Id).Select(i => new ImagePath(i.Image)).ToArray(),
            Item = listing.Item?.ToDto()
        };
    }
    public static ItemDTO ToDto(this Item item)
    {
        return new ItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            Sku = item.SKU,
            Seller = item.Seller?.ToDto()
        };
    }
    public static SellerDTO ToDto(this Seller seller)
    {
        return new SellerDTO
        {
            Id = seller.Id,
            Name = seller.Name,
            Image = seller.Image,
        };
    }
    public static CartItemDTO ToDto(this CartItem cartItem)
    {
        return new CartItemDTO
        {
            Id = cartItem.Id,
            AddDate = cartItem.AddDate,
            Quantity = cartItem.Quantity,
            Listing = cartItem.Listing?.ToDto(),
        };
    }
    public static MemberDTO ToDto(this UserSeller userSeller)
    {
        return new MemberDTO { Role = userSeller.Role, User = userSeller.User.ToDto() };
    }
    public static UserDTO ToDto(this User user)
    {
        return new UserDTO
        {
            Id = user.Id, 
            Username = user.Username, 
            Email = user.Email, 
            ContactName = user.ContactName, 
            Phone = user.Phone 
        };
    }
    public static UserDTO ToFullDto(this User user)
    {
        return new UserDTO
        {
            Id = user.Id, 
            Username = user.Username, 
            Email = user.Email, 
            ContactName = user.ContactName, 
            Phone = user.Phone,
            Street = user.Street,
            City = user.City,
            State = user.State,
            Zip = user.Zip
        };
    }

    public static TransactionItemDTO ToDto(this TransactionItem transactionItem)
    {
        return new TransactionItemDTO
        {
            Id = transactionItem.Id,
            Item = transactionItem.Item.ToDto(),
            Quantity = transactionItem.Quantity,
            TotalPrice = transactionItem.TotalPrice,
            UnitPrice = transactionItem.UnitPrice,
        };
    }
    public static TransactionDTO ToDto(this Transaction transaction)
    {
        return new TransactionDTO
        {
            Id = transaction.Id,
            Last4 = transaction.Last4,
            TotalPrice = transaction.TotalPrice,
            Items = transaction.Items.Select(i => i.ToDto()),
            PurchaseDate = transaction.PurchaseDate
        };
    }
}
