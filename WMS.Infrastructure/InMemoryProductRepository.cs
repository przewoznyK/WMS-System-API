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
            Console.WriteLine("Add product " + product.Name);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? GetProductById(Guid id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }
    }
}
