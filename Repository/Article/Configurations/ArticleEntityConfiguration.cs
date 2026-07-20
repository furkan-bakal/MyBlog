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

            // Content bilinçli olarak sınırsız (text) bırakıldı; bir blog yazısına karakter üst sınırı koymuyoruz.
            builder.Property(a => a.Content).IsRequired();
        }
    }
}
