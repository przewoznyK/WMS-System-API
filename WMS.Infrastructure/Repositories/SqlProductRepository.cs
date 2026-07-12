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

        public Task Add(Product product)
        {
            _wmsDbContext.Products.Add(product);
            return Task.CompletedTask;
        }

        public Task Delete(Product product)
        {
            _wmsDbContext.Products.Remove(product);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _wmsDbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _wmsDbContext.Products.FindAsync(id);
        }

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            return await _wmsDbContext.Products.FirstOrDefaultAsync(l => l.Sku == sku);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _wmsDbContext.Products.FirstOrDefaultAsync(l => l.Name == name);
        }

        public async Task<IEnumerable<string>> GetNamesAsync()
        {
            return await _wmsDbContext.Products
                .Select(p => p.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            return await _wmsDbContext.Products
                .AnyAsync(p => p.Sku == sku);
        }

        public async Task<Product?> GetBySkuOrNameAsync(string searchTerm)
        {
            return await _wmsDbContext.Products.FirstOrDefaultAsync(p => p.Sku == searchTerm || p.Name == searchTerm);
        }
    }
}