using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// The request used to create a listing.
/// </summary>
public class CreateListingRequest
{
    /// <summary>
    /// The display name of the listing.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
    /// <summary>
    /// The item id the listing belongs to.
    /// </summary>
    public int Item { get; set; }
    /// <summary>
    /// The available quantity of the listing.
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// The description of the listing.
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }
    /// <summary>
    /// Is the listing available to the public?
    /// </summary>
    public bool Active { get; set; } = true;
    /// <summary>
    /// The price per unit of the listing.
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// The tags used to search the listing.
    /// </summary>
    public IEnumerable<string> Tags { get; set; } = [];
}
