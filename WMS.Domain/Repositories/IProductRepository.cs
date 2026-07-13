using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IProductRepository
    {
        Task Add(Product product);
        Task Delete(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<string>> GetNamesAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> GetBySkuAsync(string sku);
        Task<Product?> GetByNameAsync(string name);
        Task<bool> ExistsBySkuAsync(string code);
        Task<Product?> GetBySkuOrNameAsync(string searchTerm);
        Task<int> GetCountAsync();
    }
}