using Core;
using Core.Article.Entity;

namespace Repository
{
    public interface IArticleRepository: IGenericRepository<ArticleEntity>
    {
        public Task<List<ArticleEntity>> GetAllByPaginate(int take, int skip);

        public Task<ArticleEntity?> GetByIdWithCategory(Guid id);
    }
}
