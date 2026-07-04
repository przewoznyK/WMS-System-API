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

        public IEnumerable<Product> GetAll()
        {
            return _wmsDbContext.Products.ToList();
        }
    }
}
