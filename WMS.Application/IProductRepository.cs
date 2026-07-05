using WMS.Domain;

namespace WMS.Application
{
    public interface IProductRepository
    {
        public void Add(Product product);
        public IEnumerable<Product> GetAll();
        public Product? GetProductById(Guid id);
        public void UpdateDetails(Product product, string name, string description);
    }
}