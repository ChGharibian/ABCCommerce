namespace SharedModels.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Sku { get; set; } = "";
    public Seller? Seller { get; set; }
}
