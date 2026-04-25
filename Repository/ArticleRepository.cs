using Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ArticleRepository : IBaseRepository<ArticleEntity>
    {
        private static readonly List<ArticleEntity> _articles = new List<ArticleEntity>();

        public void Add(ArticleEntity entity) => _articles.Add(entity);

        public IReadOnlyList<ArticleEntity> GetAll() => _articles;

        public ArticleEntity? GetById(int Id) => _articles.FirstOrDefault(x => x.Id == Id);

        public void Remove(int Id)
        {
            var article = GetById(Id);

            _articles.Remove(article!);
        }

        public void Update(ArticleEntity entity)
        {
            var index = _articles.FindIndex(x => x.Id == entity.Id);

            _articles[index] = entity;
        }
    }
}
