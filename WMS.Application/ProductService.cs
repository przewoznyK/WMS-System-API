using WMS.Domain;

namespace WMS.Application
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void CreateProduct(string sku, string name, string description)
        {
            Product newProduct = new(sku, name, description);
            _productRepository.Add(newProduct);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }
    }
}