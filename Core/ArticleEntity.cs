namespace Core
{
    public class ArticleEntity : BaseEntity
    {
        public string Content { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
    }
}
