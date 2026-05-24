using Core;
using Core.Category.Dto;
using System.Collections.Immutable;

namespace Service.Category
{
    public interface ICategoryService
    {
        Task<ResponseModelDto<IImmutableList<CategoryDto>>> GetAll();
        Task<ResponseModelDto<Guid>> Add(CreateCategoryDto createCategoryDto);
        Task<ResponseModelDto<CategoryWithArticlesDto?>> GetByIdWithArticles(Guid id);
        Task<ResponseModelDto<NoContent>> Update(Guid id, UpdateCategoryDto updateCategoryDto);
        Task<ResponseModelDto<NoContent>> Remove(Guid id);
    }
}
