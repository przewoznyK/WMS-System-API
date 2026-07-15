using WMS.Domain.Enums;

namespace WMS.Client.Services.Interfaces
{
    public interface ITokenStorageService
    {
        Task SaveTokenAsync(string token);
        Task<string?> GetTokenAsync();
        Task RemoveTokenAsync();
    }
}