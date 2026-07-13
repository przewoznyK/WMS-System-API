using MediatR;

namespace WMS.Application.Stocks.Commands
{
    public record ReceiveStockCommand(string ProductSku, string LocationCode, int Quantity) : IRequest<Guid>;
}