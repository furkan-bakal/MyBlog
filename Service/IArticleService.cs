using Core;
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
        ResponseModelDto<IImmutableList<ArticleDto>> GetAll();
        ResponseModelDto<int> Add(CreateArticleDto createArticleDto);
        ResponseModelDto<ArticleDto?> GetById(int id);
        ResponseModelDto<NoContent> Update(int id, UpdateArticleDto updateArticleDto);
        ResponseModelDto<NoContent> Remove(int id);
    }
}
