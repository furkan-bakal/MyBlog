using AutoMapper;
using Core.Article.Dto;
using Core.Article.Entity;
using Core.Category.Dto;
using Core.Category.Entity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArticleEntity, ArticleDto>()
            .ForMember("CreatedDate", opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForMember("UpdatedDate", opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToShortDateString() : null))
            .ForMember("DeletedDate", opt => opt.MapFrom(src => src.DeletedDate.HasValue ? src.DeletedDate.Value.ToShortDateString() : null))
            .ReverseMap();

        CreateMap<CategoryEntity, CategoryDto>()
            .ForMember("CreatedDate", opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForMember("UpdatedDate", opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToShortDateString() : null))
            .ForMember("DeletedDate", opt => opt.MapFrom(src => src.DeletedDate.HasValue ? src.DeletedDate.Value.ToShortDateString() : null))
            .ReverseMap();

        CreateMap<CategoryEntity, CategoryWithArticlesDto>();
    }
}