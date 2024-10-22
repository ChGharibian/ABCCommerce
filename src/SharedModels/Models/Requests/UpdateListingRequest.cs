using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class UpdateListingRequest
{
    public int? Quantity { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    public bool? Active { get; set; }
    public decimal? Price { get; set; }
    public string[]? AddTags { get; set; }
    public string[]? RemoveTags { get; set; }
}
