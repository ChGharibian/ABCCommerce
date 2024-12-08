using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models;
public class Transaction
{
    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string Last4 { get; set; } = null!;
    public DateTime PurchaseDate { get; set; }
    public IEnumerable<TransactionItem> Items { get; set; } = new List<TransactionItem>();
}
public class TransactionItem
{

    public int Id { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public Item Item { get; set; } = null!;
}