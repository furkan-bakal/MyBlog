using Core.Category.Dto;
using FluentValidation;


namespace Service.Category.Validations
{
    public class CategoryCreateRequestValidator: AbstractValidator<CreateCategoryDto>
    {
        public CategoryCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");
        }
    }
}
