using Core;
using Repository;
using System.Collections.Immutable;
using System.Net;

namespace Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public ResponseModelDto<IImmutableList<ArticleDto>> GetAll()
        {
            var articles = _articleRepository.GetAll().ToImmutableList();
            var tranformedArticles = articles.Select(a =>
            new ArticleDto(a.Id, a.Title, a.Content, a.Author, a.CreatedDate.ToShortDateString(), a.UpdatedDate.ToShortDateString()))
                .ToImmutableList();
            return ResponseModelDto<IImmutableList<ArticleDto>>.Success(tranformedArticles);
        }

        public ResponseModelDto<int> Add(CreateArticleDto createArticleDto)
        {
            var entity = new ArticleEntity
            {
                Id =   _articleRepository.GetAll().Count()+1,
                Content = createArticleDto.Content,
                Title = createArticleDto.Title,
                Author = createArticleDto.Author,
                CreatedDate = DateTime.Now
            };
            _articleRepository.Add(entity);
            return ResponseModelDto<int>.Success(entity.Id);
        }

        public ResponseModelDto<ArticleDto?> GetById(int id)
        {
            var article = _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<ArticleDto?>.Failure("Article not found");
            }
            var articleDto = new ArticleDto(article.Id, article.Title, article.Content, article.Author, article.CreatedDate.ToShortDateString(), article.UpdatedDate.ToShortDateString());
            return ResponseModelDto<ArticleDto?>.Success(articleDto);
        }

        public ResponseModelDto<NoContent> Remove(int id)
        {
            var article = _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<NoContent>.Failure("Article not found", HttpStatusCode.NotFound);
            }
            _articleRepository.Remove(article);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public ResponseModelDto<NoContent> Update(int id, UpdateArticleDto updateArticleDto)
        {
            var article = _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<NoContent>.Failure("Article not found", HttpStatusCode.NotFound);
            }
            var updatedArticle = new ArticleEntity
            {
                Id = id,
                Content = updateArticleDto.Content,
                Title = updateArticleDto.Title,
                Author = updateArticleDto.Author,
                UpdatedDate = DateTime.Now
            };
            _articleRepository.Update(updatedArticle);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}