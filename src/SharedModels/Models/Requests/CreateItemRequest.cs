using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class CreateItemRequest
{
    public int Seller { get; set; }
    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
}