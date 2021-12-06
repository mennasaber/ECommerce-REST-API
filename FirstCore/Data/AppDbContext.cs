using FirstCore.Data.Models;
using FirstCore.IdentityAuth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne<Order>(op => op.order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);


            modelBuilder.Entity<OrderProduct>()
             .HasOne<Product>(op => op.Product)
             .WithMany(p => p.OrderProduct)
             .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<Customer>()
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Customer>(c => c.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<Owner>()
                .WithOne(o => o.ApplicationUser)
                .HasForeignKey<Owner>(o => o.UserId);

            modelBuilder.Entity<Product>()
                .HasOne(o => o.Owner)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.OwnerId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
