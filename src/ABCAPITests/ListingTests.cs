using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using ABCCommerceDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ABCAPITests;

public class ListingTests : Xunit.IAsyncLifetime
{
    ABCCommerceContext DbContext { get; }
    ListingService ListingService { get; }
    public ListingTests()
    {
        DbContextOptionsBuilder<ABCCommerceContext> optionsBuilder = new();
        optionsBuilder.UseInMemoryDatabase(nameof(ListingTests));
        DbContext = new ABCCommerceContext(optionsBuilder.Options);
        ListingService = new(DbContext);
    }
    [Fact]
    public async Task AvailabilityNoCartItems()
    {
        var listing = new Listing
        {
            Item = Item,
            Active = true,
            Quantity = 10
        };
        DbContext.Listings.Add(listing);
        DbContext.SaveChanges();
        Assert.Equal(10, await ListingService.Availability(listing.Id));
        DbContext.Listings.Remove(listing);
        DbContext.SaveChanges();
    }
    [Fact]
    public async Task AvailabilityAndCountAvailableOneCartItem()
    {
        var listing = new Listing
        {
            Item = Item,
            Active = true,
            Quantity = 10
        };
        DbContext.Listings.Add(listing);
        DbContext.SaveChanges();
        var cartItem = new CartItem
        {
            User = User,
            Quantity = 5,
            Listing = listing,
            AddDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            
        };
        DbContext.CartItems.Add(cartItem);
        DbContext.SaveChanges();
        Assert.Equal(5, await ListingService.Availability(listing.Id));
        Assert.True(await ListingService.IsCountAvailable(cartItem.Quantity, cartItem.Id));
        DbContext.Listings.Remove(listing);
        DbContext.SaveChanges();
    }
    User? User { get; set; }
    Seller? Seller { get; set; }
    Item? Item { get; set; }
    public Task InitializeAsync()
    {
        User = new User()
        {
            Email = "test@tester.com",
            Password = "test",
        };
        Seller = new()
        {
            Name = "Foo Seller"
        };
        Item = new()
        {
            Name = "Foo",
            SKU = "Foo"
        };
        DbContext.Users.Add(User);
        DbContext.Sellers.Add(Seller);
        DbContext.Items.Add(Item);
        DbContext.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        if(Seller is not null)
        DbContext.Sellers.Remove(Seller);
        if(Item is not null)
        DbContext.Items.Remove(Item);
        DbContext.SaveChanges();
        return Task.CompletedTask;
    }
}