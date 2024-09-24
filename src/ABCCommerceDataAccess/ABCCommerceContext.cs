using ABCCommerceDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ABCCommerceDataAccess;

public class ABCCommerceContext : DbContext
{
    public ABCCommerceContext() : base()
    {
        
    }
    public ABCCommerceContext(DbContextOptions<ABCCommerceContext> options) : base(options)
    {
        
    }

    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Listing> Listings { get; set; }
}
