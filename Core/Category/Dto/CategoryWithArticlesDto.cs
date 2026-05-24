using Core.Article.Dto;

namespace Core.Category.Dto
{
    public record CategoryWithArticlesDto:CategoryDto
    {
        public List<ArticleDto>? Articles { get; init; }
    }
}
