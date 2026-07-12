using MediatR;
using WMS.Domain.Enums;

namespace WMS.Application.Stocks.Commands
{
    public record IssueStockCommand(string ProductSku, string LocationCode, int Quantity, string ReferenceNumber, IssueType IssueType) : IRequest<Guid>;
}