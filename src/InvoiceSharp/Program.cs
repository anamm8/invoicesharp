using InvoiceSharp.Data;
using InvoiceSharp.Interfaces;
using InvoiceSharp.Repositories;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=invoices.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Repositories are registered with Scoped lifetime, meaning a new instance is created per HTTP request and shared within that request.
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
