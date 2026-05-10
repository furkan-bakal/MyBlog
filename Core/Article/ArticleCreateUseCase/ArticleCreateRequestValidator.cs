using FluentValidation;

namespace Core
{
    public class ArticleCreateRequestValidator: AbstractValidator<CreateArticleDto>
    {
        public ArticleCreateRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.")
                .Length(5, 200).WithMessage("Title must be between 5 and 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100).WithMessage("Author cannot exceed 100 characters.");
        }
    }
}
