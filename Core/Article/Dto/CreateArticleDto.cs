namespace Core.Article.Dto
{
    public record CreateArticleDto(string Content, string Title, string Author, int CategoryId);
}
