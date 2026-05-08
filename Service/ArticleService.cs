using Core;
using Core.ArticleCreateUseCase;
using Repository;
using System.Collections.Immutable;
using System.Net;

namespace Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private IUnitOfWork _unitOfWork;

        public ArticleService(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModelDto<IImmutableList<ArticleDto>>> GetAll()
        {
            var articles = await _articleRepository.GetAll();
            var tranformedArticles = articles.Select(a =>
            new ArticleDto(a.Id, a.Title, a.Content, a.Author, a.CreatedDate.ToShortDateString(), a.UpdatedDate?.ToShortDateString()))
                .ToImmutableList();
            return ResponseModelDto<IImmutableList<ArticleDto>>.Success(tranformedArticles);
        }

        public async Task<ResponseModelDto<int>> Add(CreateArticleDto createArticleDto)
        {
            var entity = new ArticleEntity
            {
                Content = createArticleDto.Content,
                Title = createArticleDto.Title,
                Author = createArticleDto.Author,
                CreatedDate = DateTime.UtcNow
            };
            await _articleRepository.Add(entity);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<int>.Success(entity.Id);
        }

        public async Task<ResponseModelDto<ArticleDto?>> GetById(int id)
        {
            var article = await _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<ArticleDto?>.Failure("Article not found");
            }
            var articleDto = new ArticleDto(article.Id, article.Title, article.Content, article.Author, article.CreatedDate.ToShortDateString(), article.UpdatedDate?.ToShortDateString());
            return ResponseModelDto<ArticleDto?>.Success(articleDto);
        }

        public async Task<ResponseModelDto<NoContent>> Remove(int id)
        {
            var article = await _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<NoContent>.Failure("Article not found", HttpStatusCode.NotFound);
            }
            await _articleRepository.Remove(article);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int id, UpdateArticleDto updateArticleDto)
        {
            var article = await _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<NoContent>.Failure("Article not found", HttpStatusCode.NotFound);
            }

            article.Content = updateArticleDto.Content;
            article.Title = updateArticleDto.Title;
            article.Author = updateArticleDto.Author;
            article.UpdatedDate = DateTime.UtcNow;

            await _articleRepository.Update(article);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}