using WMS.Application;
using WMS.Domain;

namespace WMS.Infrastructure
{
    public class InMemoryProductRepository : IProductRepository
    {
        private List<Product> _products = new();

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {
            _products.Remove(product);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? GetProductById(Guid id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateDetails(Product product, string name, string description)
        {
            product.UpdateDetails(name, description);
        }
    }
}
