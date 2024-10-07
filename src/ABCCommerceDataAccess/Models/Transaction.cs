using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public User User { get; set; } = null!;
    public ICollection<TransactionItem> Items { get; set; } = new List<TransactionItem>();
}
