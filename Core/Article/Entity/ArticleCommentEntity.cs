namespace Core.Article.Entity
{
    public class ArticleCommentEntity : BaseEntity<Guid>
    {
        public Guid ArticleId { get; set; }
        public ArticleEntity Article { get; set; } = default!;
        public Guid? ParentCommentId { get; set; }
        public ArticleCommentEntity? ParentComment { get; set; }
        public ICollection<ArticleCommentEntity> Replies { get; set; } = new List<ArticleCommentEntity>();

        public string Content { get; set; } = default!;

        /// <summary>Yorumu yazanın belirttiği ad. Yorumlar anonim olduğu için doğrulanmaz.</summary>
        public string GuestName { get; set; } = default!;

        /// <summary>Opsiyoneldir ve API yanıtlarında asla dönmez; yalnızca ileride bildirim/gravatar için saklanır.</summary>
        public string? GuestEmail { get; set; }
    }
}
