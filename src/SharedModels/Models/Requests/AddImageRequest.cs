using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// Request to add an image to a listing.
/// </summary>
public class AddImageRequest
{
    /// <summary>
    /// The base64 image to add.
    /// </summary>
    [Required]
    public string Image { get; set; } = "";
    /// <summary>
    /// The file extension of the image. This should include the period (.) at the beginning.
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string FileType { get; set; } = "";
}