using Microsoft.EntityFrameworkCore;

namespace ABCCommerceDataAccess.Models;

public class TransactionItem
{
    public int Id { get; set; }
    [Precision(14,2)]
    public decimal TotalPrice { get; set; }
    [Precision(14,2)]
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
}