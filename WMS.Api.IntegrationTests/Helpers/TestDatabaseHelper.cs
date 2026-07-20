using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WMS.Infrastructure;

namespace WMS.Api.IntegrationTests.Helpers
{
    public static class TestDatabaseHelper
    {
        public static TestDatabaseContext CreateDatabase(
            this CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<WmsDbContext>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();

            return new TestDatabaseContext(scope, db);
        }
    }
}