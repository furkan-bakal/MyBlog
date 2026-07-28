namespace Core.Article.Dto
{
    public record CreateArticleCommentDto(
        string Content,
        string GuestName,
        string? GuestEmail,
        Guid? ParentCommentId);
}
