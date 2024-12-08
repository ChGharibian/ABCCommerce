using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCCommerceDataAccess.Models;
public class Transaction
{
    public int Id { get; set; }
    [Precision(14,2)]
    public decimal TotalPrice { get; set; }
    public int UserId { get; set; }
    [StringLength(4)]
    public string Last4 { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
    public ICollection<TransactionItem> Items { get; set; } = new List<TransactionItem>();
}
