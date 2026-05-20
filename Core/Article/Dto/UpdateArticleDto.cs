namespace Core.Article.Dto
{
    public record UpdateArticleDto(string Content, string Title, string Author, int CategoryId);
}
