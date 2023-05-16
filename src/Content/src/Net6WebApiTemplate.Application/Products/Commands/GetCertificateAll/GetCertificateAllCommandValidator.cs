using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class GetCertificateAllCommandValidator : AbstractValidator<NewsCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public GetCertificateAllCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
