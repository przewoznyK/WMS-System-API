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

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products.ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products.FindAsync(id, cancellationToken);
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products.FirstOrDefaultAsync(l => l.Sku == sku, cancellationToken);
        }

        public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products.FirstOrDefaultAsync(l => l.Name == name, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products
                .Select(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products
                .AnyAsync(p => p.Sku == sku, cancellationToken);
        }

        public async Task<Product?> GetBySkuOrNameAsync(string searchTerm, CancellationToken cancellationToken)
        {
            return await _wmsDbContext.Products.FirstOrDefaultAsync(p => p.Sku == searchTerm || p.Name == searchTerm, cancellationToken);
        }

        public Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return _wmsDbContext.Products.CountAsync(cancellationToken);
        }
    }
}