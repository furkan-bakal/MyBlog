using Core;

namespace Repository.Article
{
    public class ArticleRepositoryInMemory
    {
        private static readonly List<ArticleEntity> _articles = new List<ArticleEntity>();

        public void Add(ArticleEntity articleEntity) => _articles.Add(articleEntity);

        public IReadOnlyList<ArticleEntity> GetAll() => _articles;

        public ArticleEntity? GetById(int Id) => _articles.FirstOrDefault(x => x.Id == Id);

        public void Remove(ArticleEntity articleEntity) => _articles.Remove(articleEntity);

        public void Update(ArticleEntity articleEntity)
        {
            var index = _articles.FindIndex(x => x.Id == articleEntity.Id);
            _articles[index] = articleEntity;
        }
    }
}
