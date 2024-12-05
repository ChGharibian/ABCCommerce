using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models;
public class Member
{
    public User User { get; set; }
    public string Role { get; set; }
}
public class User
{

    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Street { get; set; } = "";
    public string? StreetPlus { get; set; }
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string Zip { get; set; } = "";
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; } = "";
}