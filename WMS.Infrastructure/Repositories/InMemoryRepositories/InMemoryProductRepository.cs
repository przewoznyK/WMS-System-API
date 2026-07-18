using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private List<Product> _products = new();

        public Task Add(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task Delete(Product product)
        {
            _products.Remove(product);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken)
        {
            var exists = _products.Any(p => p.Sku == sku);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Product>>(_products);
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(product);
        }

        public Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return Task.FromResult(_products.FirstOrDefault(x => x.Name == name));
        }

        public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
        {
            var product = _products.FirstOrDefault(x => x.Sku == sku);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_products.Select(p => p.Name));
        }

        public Task UpdateDetailsAsync(Product product, string name, string description, CancellationToken cancellationToken)
        {
            product.UpdateDetails(name, description);
            return Task.CompletedTask;
        }

        public Task<Product?> GetBySkuOrNameAsync(string searchTerm, CancellationToken cancellationToken)
        {
            var product = _products.FirstOrDefault(p => p.Sku == searchTerm || p.Name.Contains(searchTerm));
            return Task.FromResult(product);
        }

        public Task<int> GetCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_products.Count());
        }
    }
}
