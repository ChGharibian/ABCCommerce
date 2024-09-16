using ABCCommerceDataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ABCCommerceDB>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ABC")));
builder.Services.AddLogging();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
