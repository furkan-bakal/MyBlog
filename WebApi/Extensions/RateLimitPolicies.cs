namespace WebApi.Extensions
{
    /// <summary>Rate limit politika adları; Program.cs ile controller arasında sabit paylaşılır.</summary>
    public static class RateLimitPolicies
    {
        public const string ArticleLike = "article-like";
        public const string ArticleComment = "article-comment";
    }
}
