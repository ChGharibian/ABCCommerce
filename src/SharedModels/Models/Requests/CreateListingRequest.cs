using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class CreateListingRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
    public int Item { get; set; }
    public int Quantity { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
    public decimal Price { get; set; }
    public IEnumerable<string> Tags { get; set; } = [];
}
