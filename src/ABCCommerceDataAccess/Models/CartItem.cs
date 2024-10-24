using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class CartItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime AddDate { get; set; }
    public User User { get; set; } = null!;
    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
    public int Quantity { get; set; }
}
