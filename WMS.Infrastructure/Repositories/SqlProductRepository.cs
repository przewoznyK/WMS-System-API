using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly WmsDbContext _wmsDbContext;

        public SqlProductRepository(WmsDbContext wmsDbContext)
        {
            _wmsDbContext = wmsDbContext;
        }

        public async Task AddAsync(Product product)
        {
            _wmsDbContext.Products.Add(product);
            await _wmsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _wmsDbContext.Products.Remove(product);
            await _wmsDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _wmsDbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _wmsDbContext.Products.FindAsync(id);
        }

        public async Task UpdateDetailsAsync(Product product, string name, string description)
        {
            product.UpdateDetails(name, description);
            await _wmsDbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            return await _wmsDbContext.Products
                .AnyAsync(p => p.Sku == sku);
        }
    }
}