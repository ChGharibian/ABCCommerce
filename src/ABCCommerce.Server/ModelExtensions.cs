using System.Runtime.InteropServices;
using ListingDTO = SharedModels.Models.Listing;
using ItemDTO = SharedModels.Models.Item;
using SellerDTO = SharedModels.Models.Seller;
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
            Images = listing.Images.Select(i => (ImagePath)i.Image).ToArray(),
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
}