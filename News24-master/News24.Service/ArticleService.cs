using System;
using News24.Data.Infrastructure;
using News24.Data.Repositories;
using News24.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using News24.Service.Infrastructure;

namespace News24.Service
{
    public interface IArticleService
    {
        Article GetArticle(int id);

        List<Article> GetArticles();

        IEnumerable<ValidationResult> CanAddArticle(Article newArticle);

        void CreateArticle(Article article);

        void UpdateArticle(Article article);

        void DeleteArticle(Article article);

        IEnumerable<Article> Find(ArticleFilterModel filterModel, int skip, int count);

        IEnumerable<Article> GetArticlesByKeyWord(string keyWord);

    }
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ValidationResult> CanAddArticle(Article newArticle)
        {
            Article article;
            if (newArticle.ArticleId == 0)
            {
                article = _articleRepository.Get(x => x.Head == newArticle.Head);
            }
            else
            {
                article = _articleRepository.Get(
                    x => x.Head == newArticle.Head && x.ArticleId != newArticle.ArticleId);
            }
            if (article != null)
            {
                yield return new ValidationResult("Статья с данным заголовком уже существует!!");
            }
        }


        public void CreateArticle(Article article)
        {
            try
            {
                _articleRepository.Add(article);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public void DeleteArticle(Article article)
        {
            _articleRepository.Delete(article);
            _unitOfWork.Commit();
        }

        public Article GetArticle(int id)
        {
            var article = _articleRepository.GetById(id);
            return article;
        }

        public List<Article> GetArticles() => _articleRepository.GetAll().ToList();

        public IEnumerable<Article> Find(ArticleFilterModel filterModel, int skip,int count)
        {
            if (filterModel == null)
            {
                throw new ArgumentNullException(nameof(filterModel));
            }

            var articles = _articleRepository.GetMany(filterModel.GetExpression())
                .OrderByDescending(x => x.PublicationDate).Skip(skip).Take(count);
            return articles;
        }

        public void UpdateArticle(Article article)
        {
            _articleRepository.Update(article);
            _unitOfWork.Commit();
        }

        public IEnumerable<Article> GetArticlesByKeyWord(string keyWord)
        {
            if (string.IsNullOrWhiteSpace(keyWord))
            {
                return GetArticles();
            }

            var articles = _articleRepository.GetMany(x =>
                    x.Head.Contains(keyWord)
                    || x.Body.Contains(keyWord)
                    || x.Category.Name.Contains(keyWord)
                    || x.Tags.Any(z => z.Value.Contains(keyWord)))
                .OrderByDescending(x => x.PublicationDate);

            return articles;
        }
    }
}
