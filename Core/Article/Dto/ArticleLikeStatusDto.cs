namespace Core.Article.Dto
{
    /// <summary>
    /// IsLikedByMe süreç belleğindeki ziyaretçi hafızasından gelir; uygulama yeniden
    /// başladığında sıfırlanır. LikeCount ise veritabanında kalıcıdır.
    /// </summary>
    public record ArticleLikeStatusDto(int LikeCount, bool IsLikedByMe);
}
