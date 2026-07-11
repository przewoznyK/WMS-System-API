using MediatR;

namespace WMS.Application.Products.Commands
{
    public record MoveStockCommand(string ProductSku, string SourceLocationCode, string DestinationLocationCode, int Quantity) : IRequest;
}