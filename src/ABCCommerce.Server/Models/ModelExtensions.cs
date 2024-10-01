using ABCCommerceDataAccess.Models;
using System.Runtime.InteropServices;


public static class ModelExtensions
{
    public static BasicSellerDTO ToBasicDto(this Seller seller)
    {
        return new BasicSellerDTO(seller.Id, seller.Name);
    }
    public static SellerDTO ToDto(this Seller seller)
    {
        return new SellerDTO(seller.Id, seller.Name, seller.Image, seller.Items.SelectMany(l => l.Listings).Select(l => l.ToDto()));
    }
    public static ItemDTO ToDto(this Item item)
    {
        return new ItemDTO(item.Id, item.Name, item.SKU);
    }
    public static ListingDTO ToDto(this Listing listing)
    {
        return new ListingDTO(listing.Id, listing.Tags, listing.Item.ToDto(), listing.Description, listing.PricePerUnit, listing.Quantity, listing.ListingDate, listing.Active);
    }
}

public record BasicSellerDTO(int Id, string Name);
public record ItemDTO(int Id, string Name, string Sku);
public class SellerDTO
{
    public SellerDTO(int id, string name, string? image, IEnumerable<ListingDTO> listings)
    {
        Id = id;
        Name = name;
        Image = image;
        Listings = listings;
    }

    public int Id { get; }
    public string Name { get; }
    [Image]
    public string? Image { get; set; }
    public IEnumerable<ListingDTO> Listings { get; }
}
public record ListingDTO(
    int Id, 
    string[] Tags, 
    ItemDTO ItemDTO, 
    string? Description, 
    decimal PricePerUnit, 
    int Quantity, 
    DateTime ListingDate, 
    bool Active);