using MediatR;

namespace Net6WebApiTemplate.Application.Auth.Commands.SignIn
{
    public class LoginWithEmailCommand : IRequest<AuthResult>
    {
        public string email { get; set; }
    }
}