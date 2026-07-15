using WMS.Application.Authentication.Requests;
using WMS.Application.Authentication.Responses;
using WMS.Client.Services.Interfaces;
namespace WMS.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApiClientService _apiClientService;
        private readonly ITokenStorageService _tokenStorage;

        public AuthService(ApiClientService apiClientService, ITokenStorageService tokenStorage)
        {
            _apiClientService = apiClientService;
            _tokenStorage = tokenStorage;
        }

        public Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            return _apiClientService.PostAsync<LoginRequest, LoginResponse>("api/auth/login", request, ct);
        }
    }
}