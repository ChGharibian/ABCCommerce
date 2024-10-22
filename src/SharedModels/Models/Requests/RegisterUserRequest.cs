using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;

public class RegisterUserRequest
{
    [EmailAddress]
    [Required]
    [StringLength(50)]
    public string Email { get; set; } = "";
    [StringLength(50)]
    public string? Username { get; set; }
    [Required]
    [StringLength(50)]
    public string Password { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Street { get; set; } = "";
    [StringLength(50)]
    public string? StreetPlus { get; set; }
    [Required]
    [StringLength(50)]
    public string City { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string State { get; set; } = "";
    [Required]
    [StringLength(50)]
    public string Zip { get; set; } = "";

}