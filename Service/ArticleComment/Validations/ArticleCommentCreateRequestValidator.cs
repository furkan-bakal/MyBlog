using Core.Article.Dto;
using FluentValidation;

namespace Service.ArticleComment.Validations
{
    public class ArticleCommentCreateRequestValidator : AbstractValidator<CreateArticleCommentDto>
    {
        public ArticleCommentCreateRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MaximumLength(2000).WithMessage("Yorum en fazla 2000 karakter olabilir.");

            RuleFor(x => x.GuestName)
                .NotEmpty().WithMessage("İsim alanı zorunludur.")
                .Length(2, 50).WithMessage("İsim 2 ile 50 karakter arasında olmalıdır.");

            // E-posta opsiyonel; girilmişse formatı doğrulanır.
            RuleFor(x => x.GuestEmail)
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(256).WithMessage("E-posta en fazla 256 karakter olabilir.")
                .When(x => !string.IsNullOrWhiteSpace(x.GuestEmail));
        }
    }
}
