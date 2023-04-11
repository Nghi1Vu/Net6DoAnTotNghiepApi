using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class NewsCommandValidator : AbstractValidator<NewsCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public NewsCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
