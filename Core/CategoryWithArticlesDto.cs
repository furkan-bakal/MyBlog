using System.Collections.Immutable;

namespace Core
{
    public record CategoryWithArticlesDto(int Id, string Name, IImmutableList<ArticleDto> Articles);
}
