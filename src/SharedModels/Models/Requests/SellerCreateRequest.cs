using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class SellerCreateRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
}
