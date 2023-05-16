using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class GetCertificateByUserCommandValidator : AbstractValidator<NewsCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public GetCertificateByUserCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
