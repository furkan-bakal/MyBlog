namespace Core.Article.Dto
{
    public record CreateArticleDto(string Content, string Title, string Author, Guid CategoryId);
}
