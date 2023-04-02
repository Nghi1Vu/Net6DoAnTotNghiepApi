using MediatR;
using Net6WebApiTemplate.Application.Common.Exceptions;
using Net6WebApiTemplate.Application.Common.Interfaces;
//using Net6WebApiTemplate.Application.Products.Interfaces;

namespace Net6WebApiTemplate.Application.Auth.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthResult>
    {
        //private readonly ISignInManager _signInManager;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly IProductRepository _productRepository;
        public SignInCommandHandler(IProductRepository productRepository, IJwtTokenManager jwtTokenManager)
        {
            //_signInManager = signInManager;
            _jwtTokenManager = jwtTokenManager;
            _productRepository = productRepository;
        }

        public async Task<AuthResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // validate username & password 
            //var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
            try
            {
                await _productRepository.Update(request.Username);
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
            AuthResult response = new AuthResult();
            // Generate JWT token response if validation successful
            //AuthResult response = await _jwtTokenManager.GenerateClaimsTokenAsync(request.Username, cancellationToken);

            return response;
        }
    }
}