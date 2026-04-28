using Core;

namespace Repository
{
    public interface IArticleRepository
    {
        void Add(ArticleEntity articleEntity);
        IReadOnlyList<ArticleEntity> GetAll();
        ArticleEntity? GetById(int Id);
        void Remove(ArticleEntity articleEntity);
        void Update(ArticleEntity articleEntity);
    }
}
