namespace Core.Article.Dto
{
    public record ArticleImageDto(Guid Id, Guid ArticleId, string FileName, string OriginalFileName, string Path, string ContentType, long FileSize, string CreatedDate);
}
