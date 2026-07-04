using Microsoft.EntityFrameworkCore;
using WMS.Domain;

namespace WMS.Infrastructure
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
