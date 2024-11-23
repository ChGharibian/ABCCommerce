using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// The request used to create a seller.
/// </summary>
public class SellerCreateRequest
{
    /// <summary>
    /// The name of the seller.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
    /// <summary>
    /// The owner of the seller.
    /// </summary>
    [Required]
    public int UserId { get; set; }
}
