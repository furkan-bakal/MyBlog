namespace Core.Article.Entity
{
    public class ArticleImageEntity : BaseEntity<Guid>
    {
        public Guid ArticleId { get; set; }
        public ArticleEntity Article { get; set; } = default!;

        /// <summary>Diskte tutulan dosyanın adı (örn. 3f2a...c9.jpg)</summary>
        public string FileName { get; set; } = default!;

        /// <summary>İstemcinin yüklediği orijinal dosya adı</summary>
        public string OriginalFileName { get; set; } = default!;

        /// <summary>wwwroot köküne göre servis edilen yol (örn. /uploads/articles/{articleId}/{fileName})</summary>
        public string Path { get; set; } = default!;

        public string ContentType { get; set; } = default!;
        public long FileSize { get; set; }
    }
}
