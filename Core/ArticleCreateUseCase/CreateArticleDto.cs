namespace Core.ArticleCreateUseCase
{
    public record CreateArticleDto(string Content, string Title, string Author, int CategoryId);
}
