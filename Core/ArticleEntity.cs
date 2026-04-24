namespace Core
{
    public class ArticleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
    }
}
