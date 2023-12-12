using CustomerInvoice_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CustomerInvoice_WebApp.Data
{
    public sealed class CustomerInvoiceDbContext : DbContext
    {
        public CustomerInvoiceDbContext(DbContextOptions<CustomerInvoiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId);

            modelBuilder.Entity<Invoice>()
               .Property(i => i.Status)
               .HasConversion<int>();

            modelBuilder.Entity<Invoice>()
               .HasIndex(i => i.Date);

            modelBuilder.Entity<Customer>()
               .Property(c => c.Email)
               .IsRequired()
               .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
               .HasIndex(c => c.Email)
               .IsUnique();

            modelBuilder.Entity<Customer>()
               .HasMany(c => c.Invoices)
               .WithOne(i => i.Customer)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
               .OwnsOne(c => c.Address);
  
            base.OnModelCreating(modelBuilder);
        }
    }
}