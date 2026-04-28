using AutoMapper;
using Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArticleEntity, ArticleDto>();
        CreateMap<CreateArticleDto, ArticleEntity>();
        CreateMap<UpdateArticleDto, ArticleEntity>();
    }
}