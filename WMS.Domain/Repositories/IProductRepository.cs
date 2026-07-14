using WMS.Domain.Entities;

namespace WMS.Domain.Repositories
{
    public interface IProductRepository
    {
        Task Add(Product product);
        Task Delete(Product product);
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken);
        Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<bool> ExistsBySkuAsync(string code, CancellationToken cancellationToken);
        Task<Product?> GetBySkuOrNameAsync(string searchTerm, CancellationToken cancellationToken);
        Task<int> GetCountAsync(CancellationToken cancellationToken);
    }
}