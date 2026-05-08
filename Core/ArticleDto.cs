namespace Core
{
    public record ArticleDto(int Id, string Title, string Content, string Author, string CreatedDate, string? UpdatedDate);
}
