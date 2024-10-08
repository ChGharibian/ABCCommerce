using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;

public class User
{
    public int Id { get; set; }
    [StringLength(256)]
    public string Password { get; set; } = "";
    [StringLength(100)]
    public string Username { get; set; } = "";
    [StringLength(50)]
    public string Roles { get; set; } = "";
    [StringLength(100)]
    public string Street { get; set; } = "";
    [StringLength(25)]
    public string? StreetPlus { get; set; }
    [StringLength(100)]
    public string City { get; set; } = "";
    [StringLength(25)]
    public string State { get; set; } = "";
    [StringLength(25)]
    public string Zip { get; set; } = "";
    [StringLength(50)]
    public string? ContactName { get; set; }
    [StringLength(50)]
    public string? Phone { get; set; }
    [StringLength(300)]
    public string Email { get; set; } = "";
    public ICollection<UserSeller> UserSellers { get; set; } = new List<UserSeller>();
    public ICollection<CartItem> Cart { get; set; } = new List<CartItem>();
}
