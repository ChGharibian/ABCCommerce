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
    public DbSet<ListingImage> ListingImages { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    
    public DbSet<UserSeller> UserSellers { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionItem> TransactionItems { get; set; }

    public DbSet<Image> Images { get; set; }
}
