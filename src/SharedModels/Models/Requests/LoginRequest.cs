using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models.Requests;
/// <summary>
/// The request used to get a user token.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// The username of the user.
    /// </summary>
    [StringLength(50)]
    public string Username { get; set; } = "";
    /// <summary>
    /// The password of the user.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Password { get; set; } = "";
}
