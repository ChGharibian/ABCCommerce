using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class Item
{
    public int Id { get; set; }
    [StringLength(50)]
    public string SKU { get; set; } = "";
    [StringLength(50)]
    public string Name { get; set; } = "";
    public int SellerId { get; set; }
    public Seller Seller { get; set; } = null!;
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
