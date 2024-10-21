using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Examine;
using ABCCommerceDataAccess.Models;
using Examine.Lucene;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization(options =>
{
});
builder.Services.Configure<PasswordHasherOptions>(c =>
{
});
// Add services to the container.
builder.Services.AddTransient<TokenService>();
builder.Services.AddDbContext<ABCCommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ABCDB"), b => b.MigrationsAssembly("ABCCommerce.Server")));

builder.Services.AddExamine();
builder.Services.AddExamineLuceneIndex("MyIndex");


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IImageService, DbImageService>();

builder.Services.AddHostedService<InitializeSearchIndex>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

class InitializeSearchIndex : IHostedService
{
    public ILogger<InitializeSearchIndex> Logger { get; }
    public IExamineManager ExamineManager { get; }
    public IServiceProvider Provider { get; }

    public InitializeSearchIndex(ILogger<InitializeSearchIndex> logger, IExamineManager examineManager, IServiceProvider provider)
    {
        Logger = logger;
        ExamineManager = examineManager;
        Provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = Provider.CreateScope();

        var abcDb = scope.ServiceProvider.GetRequiredService<ABCCommerceContext>();

        var index = ExamineManager.GetIndex("MyIndex");
        Logger.LogInformation("Begin Indexing");
        await foreach(var listing in abcDb.Listings.Include(l => l.Item).ThenInclude(i => i.Seller).AsAsyncEnumerable())
        {
            index.Index(listing);
        }
        Logger.LogInformation("End Indexing");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
public static class Utils
{
    public static void Index(this IIndex index, Listing listing)
    {
        index.IndexItem(new ValueSet(listing.Id.ToString(), "Listing", new Dictionary<string, object>
            {
                { nameof(Listing.Name), listing.Name },
                { nameof(Listing.Description), listing.Description ?? "" },
                { nameof(Listing.Tags), listing.Tags },
                { nameof(Item.SKU), listing.Item.SKU },
                { nameof(Listing.Active), listing.Active },
                { "ItemName", listing.Item.Name },
                { "SellerName", listing.Item.Seller.Name },
                { "Listing", listing },
            }));
    }
}