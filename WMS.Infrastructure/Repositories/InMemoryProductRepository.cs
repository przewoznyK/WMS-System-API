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

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(_products);
        }

        public Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(product);
        }

        public Task UpdateDetailsAsync(Product product, string name, string description)
        {
            product.UpdateDetails(name, description);
            return Task.CompletedTask;
        }
    }
}
