using Microsoft.Extensions.DependencyInjection;
using WMS.Infrastructure;

namespace WMS.Api.IntegrationTests
{
    public class TestDatabaseContext : IDisposable
    {
        public IServiceScope Scope { get; }
        public WmsDbContext Db { get; }

        public TestDatabaseContext(IServiceScope scope, WmsDbContext db)
        {
            Scope = scope;
            Db = db;
        }

        public void Dispose()
        {
            Scope.Dispose();
        }
    }
}
