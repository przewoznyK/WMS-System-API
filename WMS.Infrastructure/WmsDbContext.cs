using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;

namespace WMS.Infrastructure
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<WarehouseLocation> WarehouseLocations { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasIndex(x => x.Sku).IsUnique();
            modelBuilder.Entity<WarehouseLocation>().HasIndex(x => x.Code).IsUnique();
            modelBuilder.Entity<Stock>().HasIndex(x => new { x.ProductId, x.LocationId }).IsUnique();
        }
    }
}
