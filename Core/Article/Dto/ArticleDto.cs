namespace Core.Article.Dto
{
    public record ArticleDto(Guid Id, string Title, string Content, string Author, int ViewCount, int LikeCount, int CommentCount, Guid CategoryId, string CreatedDate, string? UpdatedDate, bool IsDeleted, string? DeletedDate, IReadOnlyList<ArticleImageDto> Images);
}
