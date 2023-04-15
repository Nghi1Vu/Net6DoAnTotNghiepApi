using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class NewsDetailCommandValidator : AbstractValidator<NewsDetailCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public NewsDetailCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
