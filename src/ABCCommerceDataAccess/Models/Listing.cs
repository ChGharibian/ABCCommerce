using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class Listing
{
    public int Id { get; set; }
    [Precision(14,2)]
    public decimal PricePerUnit { get; set; }
    public int Quantity { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public bool Active { get; set; }
    public DateTime ListingDate { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();

}
