using WMS.Application.Products.Request;
using WMS.Application.Products.Response;
using WMS.Client.Services.Interfaces;

namespace WMS.Client.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ApiClientService _apiClientService;
        
        public ProductService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public Task<bool> CreateAsync(CreateProductRequest request, CancellationToken ct)
        {
            return _apiClientService.PostAsync("api/products/create", request, ct);
        }
        public Task<IEnumerable<string>> GetNamesAsync(CancellationToken ct)
        {
            return _apiClientService.GetListAsync<string>("api/products/names", ct);
        }

        public Task<IEnumerable<ProductResponse>> GetSummaryAsync(CancellationToken ct)
        {
            return _apiClientService.GetListAsync<ProductResponse>("api/products/summary", ct);
        }

        public Task<ProductResponse?> GetBySkuOrNameAsync(string skuOrName, CancellationToken ct)
        {
            return _apiClientService.GetAsync<ProductResponse>($"api/products/by-sku-or-name/{skuOrName}", ct);
        }
    }
}
