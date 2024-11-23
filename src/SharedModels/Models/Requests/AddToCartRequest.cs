namespace SharedModels.Models.Requests;
/// <summary>
/// The request used when adding an item to a users cart.
/// </summary>
public class AddToCartRequest
{
    /// <summary>
    /// The id of the listing to add to cart.
    /// </summary>
    public int ListingId { get; set; }
    /// <summary>
    /// The amout of units to reuest in cart.
    /// </summary>
    public int Quantity { get; set; }
}
