using Core;
using Core.Category.Dto;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Category
{
    public interface ICategoryService
    {
        Task<ResponseModelDto<IImmutableList<CategoryDto>>> GetAll();
        Task<ResponseModelDto<Guid>> Add(CreateCategoryDto createCategoryDto);
        Task<ResponseModelDto<CategoryDto?>> GetById(Guid id);
        Task<ResponseModelDto<NoContent>> Update(Guid id, UpdateCategoryDto updateCategoryDto);
        Task<ResponseModelDto<NoContent>> Remove(Guid id);
    }
}
