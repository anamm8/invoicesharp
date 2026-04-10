using Microsoft.EntityFrameworkCore;
using InvoiceSharp.Models;

namespace InvoiceSharp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Suas tabelas
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<InvoiceModel> Invoices { get; set; }
        public DbSet<InvoiceItemModel> InvoiceItems { get; set; }
    }
}