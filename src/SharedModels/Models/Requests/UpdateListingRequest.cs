using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// The request used to update a listing.
/// </summary>
public class UpdateListingRequest
{
    /// <summary>
    /// The new quantity of the listing.
    /// </summary>
    public int? Quantity { get; set; }
    /// <summary>
    /// The new description of the listing.
    /// </summary>
    [StringLength(200)]
    public string? Description { get; set; }
    /// <summary>
    /// The new active state of the listing.
    /// </summary>
    public bool? Active { get; set; }
    /// <summary>
    /// The new price of the listing.
    /// </summary>
    public decimal? Price { get; set; }
    /// <summary>
    /// The tags to add to the listing.
    /// </summary>
    public string[]? AddTags { get; set; }
    /// <summary>
    /// The tags to remove from the listing.
    /// </summary>
    public string[]? RemoveTags { get; set; }
}
