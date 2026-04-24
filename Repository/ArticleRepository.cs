using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ArticleRepository
    {
        private static readonly List<ArticleEntity> _articles = new List<ArticleEntity>();

        public IReadOnlyList<ArticleEntity> GetAll()
        {
            return _articles;
        }

        public void Add(ArticleEntity article)
        {
            _articles.Add(article);
        }

        public void Remove(ArticleEntity article)
        {
            _articles.Remove(article);
        }

        public ArticleEntity GetById(int index)
        {
            var article = _articles.Find(x => x.Id == index);

            if (article == null)
            {
                throw new IndexOutOfRangeException("Article not found.");
            }

            return article;
        }
    }
}
