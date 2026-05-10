using Core;

namespace Repository
{
    public class ArticleRepository(AppDbContext context) : GenericRepository<ArticleEntity>(context), IArticleRepository
    {
    }
}
