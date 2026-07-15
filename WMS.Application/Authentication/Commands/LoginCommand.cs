using MediatR;
using WMS.Application.Authentication.Responses;

namespace WMS.Application.Authentication.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResponse?>;
}