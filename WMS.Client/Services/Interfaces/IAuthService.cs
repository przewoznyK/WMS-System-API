using WMS.Application.Authentication.Requests;
using WMS.Application.Authentication.Responses;

namespace WMS.Client.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
    }
}