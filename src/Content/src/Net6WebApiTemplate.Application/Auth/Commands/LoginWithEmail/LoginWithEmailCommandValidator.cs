using FluentValidation;

namespace Net6WebApiTemplate.Application.Auth.Commands.SignIn
{
    public class LoginWithEmailCommandValidator : AbstractValidator<LoginWithEmailCommand>
    {
        public LoginWithEmailCommandValidator()
        {
            RuleFor(v => v.email)
                .NotNull()
                .NotEmpty().WithMessage("email field is required.");

            //RuleFor(v => v.Password)
            //    .NotNull()
            //    .NotEmpty().WithMessage("Password field is required.");
        }
    }
}