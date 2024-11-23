using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// The request to create an item.
/// </summary>
public class CreateItemRequest
{
    /// <summary>
    /// Deprecated
    /// </summary>
    public int Seller { get; set; }
    /// <summary>
    /// The new sku of the item to add.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = "";
    /// <summary>
    /// The new name of the item to add.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";
}