using Core;
using Core.ArticleCreateUseCase;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IArticleService
    {
        Task<ResponseModelDto<IImmutableList<ArticleDto>>> GetAll();
        Task<ResponseModelDto<int>> Add(CreateArticleDto createArticleDto);
        Task<ResponseModelDto<ArticleDto?>> GetById(int id);
        Task<ResponseModelDto<NoContent>> Update(int id, UpdateArticleDto updateArticleDto);
        Task<ResponseModelDto<NoContent>> Remove(int id);
    }
}
