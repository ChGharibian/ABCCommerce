namespace SharedModels.Models.Requests;

public class AddToCartRequest
{
    public int ListingId { get; set; }
    public int Quantity { get; set; }
}
