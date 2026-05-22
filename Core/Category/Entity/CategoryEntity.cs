using Core.Article.Entity;

namespace Core.Category.Entity
{
    public class CategoryEntity : BaseEntity<Guid>
    {
        public string Name { get; set; } = default!;
        public ICollection<ArticleEntity> Articles { get; set; } = new List<ArticleEntity>();
    }
}
