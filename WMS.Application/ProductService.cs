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

        public async Task CreateProductAsync(string sku, string name, string description)
        {
            Product newProduct = new(sku, name, description);
            await _productRepository.AddAsync(newProduct);
        }

        public async Task DeleteProductAsync(Product product)
        {
            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task UpdateDetailsAsync(Product product, string name, string description)
        {
            await _productRepository.UpdateDetailsAsync(product, name, description);
        }
    }
}