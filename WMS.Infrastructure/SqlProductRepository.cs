using WMS.Application;
using WMS.Domain;

namespace WMS.Infrastructure
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly WmsDbContext _wmsDbContext;

        public SqlProductRepository(WmsDbContext wmsDbContext)
        {
            _wmsDbContext = wmsDbContext;
        }

        public void Add(Product product)
        {
            _wmsDbContext.Products.Add(product);
            _wmsDbContext.SaveChanges();
        }

        public void Delete(Product product)
        {
            _wmsDbContext.Products.Remove(product);
            _wmsDbContext.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            return _wmsDbContext.Products.ToList();
        }

        public Product? GetProductById(Guid id)
        {
            return _wmsDbContext.Products.Find(id);
        }

        public void UpdateDetails(Product product, string name, string description)
        {
            product.UpdateDetails(name, description);
            _wmsDbContext.SaveChanges();
        }
    }
}