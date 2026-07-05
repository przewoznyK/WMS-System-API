using WMS.Domain;

namespace WMS.Application
{
    public interface IProductRepository
    {
        public Task AddAsync(Product product);
        public Task DeleteAsync(Product product);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product?> GetProductByIdAsync(Guid id);
        public Task UpdateDetailsAsync(Product product, string name, string description);
    }
}