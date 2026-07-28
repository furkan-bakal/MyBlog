using Core;
using Core.Article.Dto;
using Microsoft.Extensions.Caching.Memory;
using Repository;
using Service.Visitor;

namespace Service
{
    /// <summary>
    /// Beğeniler anonim olduğu için kalıcı bir beğeni tablosu tutulmuyor: sayaç Articles
    /// tablosunda bir kolon, "kim beğendi" bilgisi ise yalnızca süreç belleğinde.
    /// Uygulama yeniden başladığında sayı korunur, tekrar beğenme engeli sıfırlanır.
    /// </summary>
    public class ArticleLikeService : IArticleLikeService
    {
        private static readonly TimeSpan LikeMemoryDuration = TimeSpan.FromHours(24);

        private readonly IArticleRepository _articleRepository;
        private readonly IVisitorIdentityService _visitorIdentityService;
        private readonly IMemoryCache _memoryCache;

        public ArticleLikeService(
            IArticleRepository articleRepository,
            IVisitorIdentityService visitorIdentityService,
            IMemoryCache memoryCache)
        {
            _articleRepository = articleRepository;
            _visitorIdentityService = visitorIdentityService;
            _memoryCache = memoryCache;
        }

        public async Task<ResponseModelDto<ArticleLikeStatusDto>> GetStatus(Guid articleId)
        {
            var likeCount = await _articleRepository.GetLikeCount(articleId);
            return ResponseModelDto<ArticleLikeStatusDto>.Success(
                new ArticleLikeStatusDto(likeCount, HasLiked(articleId)));
        }

        public async Task<ResponseModelDto<ArticleLikeStatusDto>> Like(Guid articleId)
        {
            // Zaten beğenilmişse hata değil başarı dönüyoruz: ağ tekrarında veya çift
            // tıklamada istemcinin durumu bozulmasın, sayaç da ikinci kez artmasın.
            if (HasLiked(articleId))
            {
                return await GetStatus(articleId);
            }

            await _articleRepository.IncrementLikeCount(articleId);
            _memoryCache.Set(BuildCacheKey(articleId), true, LikeMemoryDuration);

            return await GetStatus(articleId);
        }

        public async Task<ResponseModelDto<ArticleLikeStatusDto>> Unlike(Guid articleId)
        {
            // Sadece bu ziyaretçinin beğendiği kayıtlıysa azaltıyoruz; aksi halde herkes
            // sayacı istediği kadar düşürebilirdi.
            if (!HasLiked(articleId))
            {
                return await GetStatus(articleId);
            }

            await _articleRepository.DecrementLikeCount(articleId);
            _memoryCache.Remove(BuildCacheKey(articleId));

            return await GetStatus(articleId);
        }

        private bool HasLiked(Guid articleId) => _memoryCache.TryGetValue(BuildCacheKey(articleId), out _);

        private string BuildCacheKey(Guid articleId) => $"like:{articleId}:{_visitorIdentityService.GetVisitorHash()}";
    }
}
