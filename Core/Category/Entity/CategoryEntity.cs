namespace Core.Category.Entity
{
    public class CategoryEntity : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public ICollection<ArticleEntity> Articles { get; set; } = new List<ArticleEntity>();
    }
}
