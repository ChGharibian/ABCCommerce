using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class LoginRequest
{
    [StringLength(50)]
    public string Username { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Password { get; set; } = "";
}
