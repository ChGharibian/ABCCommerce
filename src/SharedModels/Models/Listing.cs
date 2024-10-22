using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models;
public class Listing
{
    public int Id {get; set;}
    public string Name { get; set;}
    public Item? Item {get; set;}
    public DateTime ListingDate {get; set;}
    public int Quantity {get; set;}
    public decimal PricePerUnit {get; set;}
    public string? Description {get; set;}
    public bool Active { get; set; }
    public ImagePath[] Images {get; set;}
    public string[] Tags {get; set;}
}
