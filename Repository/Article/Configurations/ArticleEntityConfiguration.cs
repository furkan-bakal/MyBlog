using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class ArticleEntityConfiguration:IEntityTypeConfiguration<ArticleEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleEntity> builder)
        {
            builder.HasQueryFilter(a => !a.IsDeleted);

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Author).IsRequired().HasMaxLength(100);
            builder.Property(a => a.ViewCount).IsRequired().HasDefaultValue(0);
            builder.Property(a => a.LikeCount).IsRequired().HasDefaultValue(0);
            builder.Property(a => a.CommentCount).IsRequired().HasDefaultValue(0);
            builder.Property(a => a.Content).IsRequired();
        }
    }
}
