using Microsoft.EntityFrameworkCore;
using InvoiceSharp.Models;

namespace InvoiceSharp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Override the method to prevent cascade delete when a Client is deleted, ensuring that Invoices remain intact
            modelBuilder.Entity<InvoiceModel>()
                .HasOne(i => i.Client)
                .WithMany()
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add default value for Date property in InvoiceModel to ensure it gets set to the current timestamp if not provided
            modelBuilder.Entity<InvoiceModel>()
                .Property(i => i.Date)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        // Billing entities
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<InvoiceModel> Invoices { get; set; }
        public DbSet<InvoiceItemModel> InvoiceItems { get; set; }
    }
}