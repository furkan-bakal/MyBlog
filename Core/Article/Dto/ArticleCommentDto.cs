namespace Core.Article.Dto
{
    /// <summary>
    /// Yorum ağacının tek seviyesi. GuestEmail bilinçli olarak dışarı verilmez.
    /// </summary>
    public record ArticleCommentDto(
        Guid Id,
        string GuestName,
        string Content,
        string CreatedDate,
        IReadOnlyList<ArticleCommentDto> Replies);
}
