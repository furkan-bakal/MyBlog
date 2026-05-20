using Core;
using Core.Article.Dto;
using System.Collections.Immutable;

namespace Service
{
    public interface IArticleService
    {
        Task<ResponseModelDto<IImmutableList<ArticleDto>>> GetAll();
        Task<ResponseModelDto<Guid>> Add(CreateArticleDto createArticleDto);
        Task<ResponseModelDto<ArticleDto?>> GetById(Guid id);
        Task<ResponseModelDto<NoContent>> Update(Guid id, UpdateArticleDto updateArticleDto);
        Task<ResponseModelDto<NoContent>> Remove(Guid id);
    }
}
