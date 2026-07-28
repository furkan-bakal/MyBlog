using Core;
using Core.Article.Entity;

namespace Repository
{
    public interface IArticleRepository: IGenericRepository<ArticleEntity>
    {
        public Task<List<ArticleEntity>> GetAllByPaginate(int take, int skip);

        public Task<ArticleEntity?> GetByIdWithCategory(Guid id);

        /// <summary>
        /// Görüntülenme sayacını veritabanı üzerinde atomik olarak artırır (read-modify-write yok).
        /// </summary>
        /// <returns>Etkilenen satır sayısı; makale yoksa veya silinmişse 0.</returns>
        public Task<int> IncrementViewCount(Guid id);

        public Task<int> IncrementLikeCount(Guid id);

        public Task<int> DecrementLikeCount(Guid id);

        public Task<int> IncrementCommentCount(Guid id, int by = 1);

        public Task<int> DecrementCommentCount(Guid id, int by = 1);

        public Task<int> GetLikeCount(Guid id);
    }
}
