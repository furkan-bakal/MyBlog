using Core;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ArticleService
    {
        private readonly ArticleRepository _articlesRepository;

        public ArticleService()
        {
            _articlesRepository = new ArticleRepository();
        }

        public IReadOnlyList<ArticleEntity> GetAll()
        {
            return _articlesRepository.GetAll();
        }

        public void Add(ArticleEntity article)
        {
            _articlesRepository.Add(article);
        }

        public ArticleEntity GetById(int id)
        {
            return _articlesRepository.GetById(id);
        }
    }
}
