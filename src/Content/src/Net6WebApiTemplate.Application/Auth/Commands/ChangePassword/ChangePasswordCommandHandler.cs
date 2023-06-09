using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
//using Net6WebApiTemplate.Application.Products.Interfaces;

namespace Net6WebApiTemplate.Application.Auth.Commands.SignIn
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, int>
    {
        //private readonly ISignInManager _signInManager;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly IProductRepository _productRepository;
        public ChangePasswordCommandHandler(IProductRepository productRepository, IJwtTokenManager jwtTokenManager)
        {
            //_signInManager = signInManager;
            _jwtTokenManager = jwtTokenManager;
            _productRepository = productRepository;
        }

        public async Task<int> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                int check= _productRepository.ChangePassword(request.username, request.oldpass, request.newpass);
                return check;
            }
            catch
            {
                throw new UnauthorizedException("Invalid username or password.");
            }
            // Throw exception if credential validation failed
            //if (!result.Succeeded)
            //{
            //    throw new UnauthorizedException("Invalid username or password.");
            //}
            // Generate JWT token response if validation successful
            //AuthResult response = await _jwtTokenManager.GenerateClaimsTokenAsync(request.Username, cancellationToken);
        }
    }
}