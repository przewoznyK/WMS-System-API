using MediatR;

namespace WMS.Application.Stocks.Commands
{
    public record ReceiveStockCommand(string Sku, string LocationCode, int Quantity) : IRequest<Guid>;
}