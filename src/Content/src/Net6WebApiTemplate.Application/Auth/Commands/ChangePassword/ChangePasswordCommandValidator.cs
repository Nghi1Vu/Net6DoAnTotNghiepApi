using FluentValidation;

namespace Net6WebApiTemplate.Application.Auth.Commands.SignIn
{
    public class ChangePasswordCommandValidator : AbstractValidator<SignInCommand>
    {
        public ChangePasswordCommandValidator()
        {
            //RuleFor(v => v.Username)
            //    .NotNull()
            //    .NotEmpty().WithMessage("Username field is required.");

            //RuleFor(v => v.Password)
            //    .NotNull()
            //    .NotEmpty().WithMessage("Password field is required.");
        }
    }
}