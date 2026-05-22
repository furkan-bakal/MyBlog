using Core.Category.Entity;

namespace Core.Article.Entity
{
    public class ArticleEntity : BaseEntity<Guid>
    {
        public string Content { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = default!;
    }
}
