namespace SharedModels.Models;

public class CartItem
{
    public int Id { get; init; }
    public Listing? Listing { get; init; }
    public DateTime AddDate { get; set; }
    public int Quantity { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CartItem other &&
               Id == other.Id &&
               EqualityComparer<Listing>.Default.Equals(Listing, other.Listing) &&
               AddDate == other.AddDate &&
               Quantity == other.Quantity;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Listing, AddDate, Quantity);
    }
}