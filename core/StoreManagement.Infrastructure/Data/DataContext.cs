using StoreManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreManagement.Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductSell> ProductSells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentType>()
            .HasOne(pt => pt.Store)
            .WithMany(s => s.PaymentTypes) 
            .HasForeignKey(pt => pt.IdStore)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.IdOrder, od.IdFood });

            modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.IdOrder)
            .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Food)
                .WithMany(f => f.OrderDetails)
                .HasForeignKey(od => od.IdFood)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}
