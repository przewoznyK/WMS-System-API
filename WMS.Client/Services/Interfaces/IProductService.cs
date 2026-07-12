using WMS.Application.Products.Request;
using WMS.Application.Products.Response;

namespace WMS.Client.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetSummaryAsync(CancellationToken ct);
        Task<ProductResponse?> GetBySkuOrNameAsync(string skuOrName, CancellationToken ct);
        Task<bool> CreateAsync(CreateProductRequest request, CancellationToken ct);
        Task<IEnumerable<string>> GetNamesAsync(CancellationToken ct);
    }
}