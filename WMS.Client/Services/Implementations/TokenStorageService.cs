using WMS.Client.Services.Interfaces;
using Microsoft.JSInterop;

namespace WMS.Client.Services.Implementations
{
    public class TokenStorageService : ITokenStorageService
    {
        private readonly IJSRuntime _js;

        public TokenStorageService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SaveTokenAsync(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
        }

        public async Task RemoveTokenAsync()
        {
            await _js.InvokeVoidAsync( "localStorage.removeItem", "authToken");
        }
    }
}