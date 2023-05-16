using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class GetTradeHistoryCommandValidator : AbstractValidator<NewsCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public GetTradeHistoryCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
