using FluentValidation;

namespace Net6WebApiTemplate.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotNull().WithMessage("id field is required")
                .LessThan(1).WithMessage("Invalid id.");
        }
    }
}
