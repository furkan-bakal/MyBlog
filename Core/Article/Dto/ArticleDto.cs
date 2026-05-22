namespace Core.Article.Dto
{
    public record ArticleDto(Guid Id, string Title, string Content, string Author, Guid CategoryId, string CreatedDate, string? UpdatedDate, bool IsDeleted, DateTime? DeletedDate);
}
