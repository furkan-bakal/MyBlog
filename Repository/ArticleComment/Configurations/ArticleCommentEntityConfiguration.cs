using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository
{
    public class ArticleCommentEntityConfiguration : IEntityTypeConfiguration<ArticleCommentEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleCommentEntity> builder)
        {
            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired().HasMaxLength(2000);
            builder.Property(c => c.GuestName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.GuestEmail).HasMaxLength(256);

            builder.HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.ArticleId, c.CreatedDate });
        }
    }
}
