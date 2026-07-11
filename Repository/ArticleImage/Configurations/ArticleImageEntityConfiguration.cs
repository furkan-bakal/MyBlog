using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class ArticleImageEntityConfiguration : IEntityTypeConfiguration<ArticleImageEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleImageEntity> builder)
        {
            builder.HasQueryFilter(i => !i.IsDeleted);

            builder.HasKey(i => i.Id);
            builder.Property(i => i.FileName).IsRequired().HasMaxLength(260);
            builder.Property(i => i.OriginalFileName).IsRequired().HasMaxLength(260);
            builder.Property(i => i.Path).IsRequired().HasMaxLength(500);
            builder.Property(i => i.ContentType).IsRequired().HasMaxLength(100);

            builder.HasOne(i => i.Article)
                .WithMany(a => a.Images)
                .HasForeignKey(i => i.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
