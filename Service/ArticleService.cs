using Core;
using Repository;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ArticleService
    {
        private readonly ArticleRepository _articlesRepository = new();

        public ResponseModelDto<IImmutableList<ArticleDto>> GetAll()
        {
            var articles = _articlesRepository.GetAll().ToImmutableList();

            return ResponseModelDto<IImmutableList<ArticleDto>>.Success(articles);
        }

        public void Add(ArticleDto article)
        {
            return ResponseModelDto<ArticleDto>.Success(_articlesRepository.Add(article);
        }

        public ResponseModelDto<ArticleDto?> GetById(int id)
        {
            var article = _articlesRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<ArticleDto?>.Failure("Article not found");
            }
            return ResponseModelDto<ArticleDto>.Success(article);
        }

        public void Remove(int id)
        {
            _articlesRepository.Remove(id);
        }
    }
}