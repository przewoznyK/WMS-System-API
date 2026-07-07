using MediatR;

namespace WMS.Application.Stocks.Commands
{
    public record ReceiveStockCommand(Guid ProductId, Guid LocationId, int Quantity = 1) : IRequest<Guid>;
}