using MediatR;
using WMS.Application.Authentication.Interfaces;
using WMS.Application.Authentication.Responses;

namespace WMS.Application.Authentication.Commands
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginCommandHandler(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.LoginAsync(request.Email, request.Password, cancellationToken);
        }
    }
}
