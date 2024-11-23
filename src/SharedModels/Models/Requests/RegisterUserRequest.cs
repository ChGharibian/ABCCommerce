using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// The request used when registering a new user.
/// </summary>
public class RegisterUserRequest
{
    /// <summary>
    /// The user's email address.
    /// </summary>
    [EmailAddress]
    [Required]
    [StringLength(50)]
    public string Email { get; set; } = "";
    /// <summary>
    /// The user's username.
    /// </summary>
    [StringLength(50)]
    public string? Username { get; set; }
    /// <summary>
    /// The user's password.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Password { get; set; } = "";
    /// <summary>
    /// The user's street address.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Street { get; set; } = "";
    /// <summary>
    /// Additional information on the user's street address.
    /// </summary>
    [StringLength(50)]
    public string? StreetPlus { get; set; }
    /// <summary>
    /// The user;'s city.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string City { get; set; } = "";
    /// <summary>
    /// The user's state.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string State { get; set; } = "";
    /// <summary>
    /// The user's zipcode.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Zip { get; set; } = "";

}