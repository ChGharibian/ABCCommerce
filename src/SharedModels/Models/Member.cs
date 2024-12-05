using System;
using System.Collections.Generic;
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
    public int Id { get; }
    public string Username { get; }
    public string Email { get; }
    public string? ContactName { get; }
    public string? Phone { get; }

    public User(int id, string username, string email, string? contactName, string? phone)
    {
        Id = id;
        Username = username;
        Email = email;
        ContactName = contactName;
        Phone = phone;
    }

    public override bool Equals(object? obj)
    {
        return obj is User other &&
               Id == other.Id &&
               Username == other.Username &&
               Email == other.Email &&
               ContactName == other.ContactName &&
               Phone == other.Phone;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Username, Email, ContactName, Phone);
    }
}