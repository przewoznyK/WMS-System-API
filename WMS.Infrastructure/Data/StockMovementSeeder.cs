using WMS.Domain.Entities;
using WMS.Domain.Enums;

namespace WMS.Infrastructure.Data
{
    public static class StockMovementSeeder
    {
        public static async Task SeedAsync(WmsDbContext context, List<Stock> stocks, string managerId, string workerId)
        {
            if (context.StockMovements.Any())
            {
                return;
            }

            var movements = new List<StockMovement>
            {
                new(stocks[0], OperationType.Receive, 120, managerId),
                new(stocks[0], OperationType.Issue, -20, workerId, IssueType.DamageOrScrap, "ORD-1001"),

                new(stocks[1], OperationType.Receive, 45, managerId),
                new(stocks[1], OperationType.Issue, -10, workerId, IssueType.InternalUse, "ORD-1002"),

                new(stocks[4], OperationType.Receive, 100, managerId),
                new(stocks[4], OperationType.Issue, -20, workerId, IssueType.ReturnToSupplier, "INT-0001"),

                new(stocks[6], OperationType.Receive, 20, managerId),
                new(stocks[6], OperationType.Issue, -8, workerId, IssueType.SalesOrder, "DMG-0001"),

                new(stocks[10], OperationType.Receive, 500, managerId),
                new(stocks[10], OperationType.Issue, -100, workerId, IssueType.SalesOrder, "ORD-1003")
            };

            var days = new[]
            {
                90,80,60,45,30,20,14,7,5,1
            };

            for (int i = 0; i < movements.Count; i++)
            {
                movements[i].SetCreatedAt(DateTime.UtcNow.AddDays(-days[i]));
            }

            context.StockMovements.AddRange(movements);
            await context.SaveChangesAsync();
        }
    }
}