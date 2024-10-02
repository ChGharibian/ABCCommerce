using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class ListingImage
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    [StringLength(100)]
    public string Image { get; set; } = "";
    public Listing Listing { get; set; } = null!;
}