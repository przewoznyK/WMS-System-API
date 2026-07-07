using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IProductRepository
    {
        public Task AddAsync(Product product);
        public Task DeleteAsync(Product product);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<Product?> GetByIdAsync(Guid id);
        public Task<Product?> GetBySkuAsync(string sku);
        public Task UpdateDetailsAsync(Product product, string name, string description);
        public Task<bool> ExistsBySkuAsync(string code);
    }
}