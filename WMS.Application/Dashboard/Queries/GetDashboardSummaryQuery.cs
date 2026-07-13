using MediatR;
using WMS.Application.Dashboard.Responses;

namespace WMS.Application.Dashboard.Queries
{
    public record GetDashboardSummaryQuery() : IRequest<DashboardSummaryResponse>;
}