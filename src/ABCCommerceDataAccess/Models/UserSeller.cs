using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class UserSeller
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Role { get; set; } = "";
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int SellerId { get; set; }
    public Seller Seller { get; set; } = null!;

}
