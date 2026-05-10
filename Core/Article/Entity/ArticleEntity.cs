namespace Core
{
    public class ArticleEntity : BaseEntity<int>
    {
        public string Content { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
    }
}
