using MediatR;

namespace WMS.Application.Stocks.Commands
{
    public record ReceiveStockCommand(string ProductName, string LocationCode, int Quantity) : IRequest<Guid>;
}