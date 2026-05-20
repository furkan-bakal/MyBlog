namespace Core.Article.Entity
{
    public class ArticleEntity : BaseEntity<Guid>
    {
        public string Content { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
    }
}
