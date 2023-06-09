using MediatR;

namespace Net6WebApiTemplate.Application.Auth.Commands.SignIn
{
    public class ChangePasswordCommand : IRequest<int>
    {
        public string username { get; set; }
        public string oldpass { get; set; }
        public string newpass { get; set; }
    }
}