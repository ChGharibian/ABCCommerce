using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class AddImageRequest
{
    [Required]
    public string Image { get; set; } = "";
    [Required]
    [MaxLength(10)]
    public string FileType { get; set; } = "";
}