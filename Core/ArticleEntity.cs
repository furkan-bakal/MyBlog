namespace Core
{
    public class ArticleEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = default!;
        public DateTime UpdatedDate { get; set; }
    }
}
