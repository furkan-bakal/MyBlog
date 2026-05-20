using AutoMapper;
using Core;
using Core.Article.Dto;
using Core.Article.Entity;
using Core.Category.Dto;
using Core.Category.Entity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArticleEntity, ArticleDto>()
            .ForCtorParam("CreatedDate", opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForCtorParam("UpdatedDate", opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToShortDateString() : null))
            .ReverseMap();

        CreateMap<CategoryEntity, CategoryDto>()
            .ForCtorParam("CreatedDate", opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForCtorParam("UpdatedDate", opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToShortDateString() : null))
            .ReverseMap();  
    }
}