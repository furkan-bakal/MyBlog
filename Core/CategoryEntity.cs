namespace Core
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; } = default!;
        public ICollection<ArticleEntity> Articles { get; set; } = new List<ArticleEntity>();
    }
}
