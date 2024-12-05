using System.Runtime.InteropServices;
using ListingDTO = SharedModels.Models.Listing;
using ItemDTO = SharedModels.Models.Item;
using SellerDTO = SharedModels.Models.Seller;
using CartItemDTO = SharedModels.Models.CartItem;
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
}
