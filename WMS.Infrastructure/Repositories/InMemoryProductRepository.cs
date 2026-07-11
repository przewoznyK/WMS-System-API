using WMS.Domain.Entities;
using WMS.Domain.Repositories;

namespace WMS.Infrastructure.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private List<Product> _products = new();

        public Task AddAsync(Product product)
        {
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _products.Remove(product);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsBySkuAsync(string sku)
        {
            var exists = _products.Any(p => p.Sku == sku);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products);
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(product);
        }

        public Task<Product?> GetByNameAsync(string name)
        {
            return Task.FromResult(_products.FirstOrDefault(x => x.Name == name));
        }

        public Task<Product?> GetBySkuAsync(string sku)
        {
            var product = _products.FirstOrDefault(x => x.Sku == sku);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<string>> GetNamesAsync()
        {
            return Task.FromResult(_products.Select(p => p.Name));
        }

        public Task UpdateDetailsAsync(Product product, string name, string description)
        {
            product.UpdateDetails(name, description);
            return Task.CompletedTask;
        }

        public Task<Product?> GetBySearch(string searchTerm)
        {
            var product = _products.FirstOrDefault(p => p.Sku == searchTerm || p.Name.Contains(searchTerm));
            return Task.FromResult(product);
        }
    }
}
