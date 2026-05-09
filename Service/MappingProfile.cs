using AutoMapper;
using Core;
using Core.ArticleCreateUseCase;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArticleEntity, ArticleDto>()
            .ForCtorParam("CreatedDate", opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForCtorParam("UpdatedDate", opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToShortDateString() : null))
            .ReverseMap();
    }
}