using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class GetDKHPByTKBCommandValidator : AbstractValidator<NewsCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public GetDKHPByTKBCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
