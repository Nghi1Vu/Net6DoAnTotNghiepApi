using FluentValidation;
using Microsoft.Extensions.Localization;
using Net6WebApiTemplate.Application.Products.Dto;

namespace Net6WebApiTemplate.Application.Products.Commands.CreateProduct
{
    public class GetStudentClassCommandValidator : AbstractValidator<StudenClass>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public GetStudentClassCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

        }
    }
}
