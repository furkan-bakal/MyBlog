namespace Core
{
    public record ArticleDto(Guid Id, string Title, string Content, string Author, string CreatedDate, string? UpdatedDate, bool IsDeleted, DateTime? DeletedDate);
}
