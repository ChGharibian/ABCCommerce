using ABCCommerce.Server.Services;
using ABCCommerceDataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });
});
// Add services to the container.
//builder.Services.AddDbContext<ABCCommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ABCDB"), b => b.MigrationsAssembly("ABCCommerce.Server")));
builder.Services.AddDbContext<ABCCommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ABCDB"), b => b.MigrationsAssembly("ABCCommerce.Server")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IImageService, DbImageService>();

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

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
