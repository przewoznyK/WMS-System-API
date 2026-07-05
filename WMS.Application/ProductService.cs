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

        public void DeleteProduct(Product product)
        {
            _productRepository.Delete(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product GetProductById(Guid id)
        {
            return _productRepository.GetProductById(id);
        }

        public void UpdateDetails(Product product, string name, string description)
        {
            _productRepository.UpdateDetails(product, name, description);
        }
    }
}