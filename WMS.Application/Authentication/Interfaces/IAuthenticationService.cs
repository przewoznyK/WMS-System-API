using WMS.Application.Authentication.Responses;

namespace WMS.Application.Authentication.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse?> LoginAsync(string email, string password, CancellationToken cancellationToken);
    }
}
