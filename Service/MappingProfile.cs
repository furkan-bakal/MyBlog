using AutoMapper;
using Core;
using Core.ArticleCreateUseCase;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ArticleEntity, ArticleDto>();
        CreateMap<CreateArticleDto, ArticleEntity>();
        CreateMap<UpdateArticleDto, ArticleEntity>();
    }
}