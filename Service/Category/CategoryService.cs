using AutoMapper;
using Core;
using Core.Category.Dto;
using Core.Category.Entity;
using Repository;
using Repository.Category;
using System.Collections.Immutable;
using System.Net;

namespace Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModelDto<IImmutableList<CategoryDto>>> GetAll()
        {
            var categories = (await _categoryRepository.GetAll()).ToList();

            var tranformedCategories = _mapper.Map<List<CategoryEntity>, List<CategoryDto>>(categories).ToImmutableList();
            return ResponseModelDto<IImmutableList<CategoryDto>>.Success(tranformedCategories);
        }

        public async Task<ResponseModelDto<Guid>> Add(CreateCategoryDto createCategoryDto)
        {
            var entity = new CategoryEntity
            {
                Name = createCategoryDto.Name
            };
            await _categoryRepository.Add(entity);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<Guid>.Success(entity.Id);
        }

        public async Task<ResponseModelDto<CategoryDto?>> GetById(Guid id)
        {
            var category = await _categoryRepository.GetById(id);

            var categoryDto = _mapper.Map<CategoryEntity, CategoryDto>(category!);
            return ResponseModelDto<CategoryDto?>.Success(categoryDto);
        }

        public async Task<ResponseModelDto<NoContent>> Remove(Guid id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category is null)
            {
                return ResponseModelDto<NoContent>.Failure("Category not found", HttpStatusCode.NotFound);
            }
            await _categoryRepository.Remove(category);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> Update(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryRepository.GetById(id);
            if (category is null)
            {
                return ResponseModelDto<NoContent>.Failure("Category not found", HttpStatusCode.NotFound);
            }

            category.Name = updateCategoryDto.Name;

            await _categoryRepository.Update(category);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
