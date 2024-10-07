using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class UserSeller
{
    public int Id { get; set; }
    public string Role { get; set; } = "";
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int SellerId { get; set; }
    public Seller Seller { get; set; } = null!;

}
