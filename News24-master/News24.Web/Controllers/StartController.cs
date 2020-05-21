using AutoMapper;
using News24.Model;
using News24.Service;
using News24.Service.Infrastructure;
using News24.Web.Models;
using News24.Web.ViewModels.ArticleViewModel;
using News24.Web.ViewModels.StartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace News24.Web.Controllers
{
    public class StartController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private const int _pageSize = 4;

        public StartController(IArticleService articleService, ICategoryService categoryService, ITagService tagService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }
        // GET: Home
        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            var categories = _categoryService.GetCategories();
            var tags = _tagService.GetDistinctTags();
            var articles = _articleService.GetArticles();
            var mappCategories = categories.Select(Mapper.Map<Category, CategoryViewModel>).ToList();
            var mappArticles = articles.Select(Mapper.Map<Article, ArticleViewModel>).Skip((page - 1) * _pageSize).Take(_pageSize).Reverse().ToList();
            var mappLastArticles = articles.Select(Mapper.Map<Article, ArticleViewModel>).OrderByDescending(x => x.PublicationDate).Take(5).ToList();
            var pager = new Pager(page, articles.Count(), _pageSize);
            var model = new IndexViewModel
            {
                Articles = mappArticles,
                Tags = tags,
                LastArticles = mappLastArticles,
                Categories = mappCategories,
                Pager = pager
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult GetArticles(int page = 1, string category = null)
        {
            if (!String.IsNullOrEmpty(category))
            {
                Session["Category"] = null;
            }
            if (Session["Category"] != null)
            {
                category = Session["Category"].ToString();
            }
            Session["Category"] = category;
            var articles = _articleService.GetArticles();
            if (!String.IsNullOrEmpty(category))
            {
                articles = articles.Where(x => x.Category.Name == category).ToList();
            }
            var mappArticles = articles.Select(Mapper.Map<Article, ArticleViewModel>).Skip((page - 1) * _pageSize).Take(_pageSize).Reverse().ToList();
            var pager = new Pager(page, articles.Count(), _pageSize);
            var model = new IndexViewModel
            {
                Articles = mappArticles,
                Pager = pager,
                Category = category
            };
            return PartialView("_Articles", model);
        }
        //public ActionResult Autocomplete(string term)
        //{
        //    try
        //    {
        //        //var model = _articleService.GetArticles() // your data here
        //        //    .Where(p => p.Tags.ForEach(x=>x.Value.Contains(term)))
        //        //    .Take(10)
        //        //    .Select(p => new
        //        //    {
        //        //// jQuery UI needs the label property to function 
        //        //    label = p.Article.Head.Trim()
        //        //    });

        //        //// Json returns [{"label":value}]
        //        //return Json(model, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //          return Json("{'ex':'Exception'}");
        //    }
        //}

        [HttpGet]
        public ActionResult Details(int id)
        {
            var article = _articleService.GetArticle(id);
            _articleService.AddView(article);
            var model = Mapper.Map<Article, ArticleDetailsViewModel>(article);

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult GetLastPosted()
        {
            var articles = _articleService.GetArticles();
            var model = articles.Select(Mapper.Map<Article, ArticleViewModel>).OrderByDescending(x => x.PublicationDate).Take(8).ToList();
            return PartialView("_LastPosted", model);
        }

        [ChildActionOnly]
        public ActionResult GetTags()
        {
            var tags = _tagService.GetDistinctTags();
            var model = tags.Select(x => new KeyValuePair<int, string>(x.TagId, x.Value));
            return PartialView("_LastPosted", model);
        }

        [ChildActionOnly]
        public ActionResult GetCategories()
        {
            var categories = _categoryService.GetCategories();
            var model = categories.Select(Mapper.Map<Category, CategoryViewModel>).ToList();
            return PartialView("_CategoriesInFooter", model);
        }

        [ChildActionOnly]
        public ActionResult GetCategoriesHeader()
        {
            var categories = _categoryService.GetCategories();
            var model = categories.Select(Mapper.Map<Category, CategoryViewModel>).ToList();
            return PartialView("_CategoriesInHeader", model);
        }

        [ChildActionOnly]
        public ActionResult GetMostPopular()
        {
            var articles = _articleService.GetPopular();
            var model = articles.Select(Mapper.Map<Article, ArticleViewModel>).Take(6).ToList();
            return PartialView("_MostPopular", model);
        }

        [ChildActionOnly]
        public ActionResult GetTrendings(int way)
        {
            var articles = _articleService.GetPopular();
            var model = articles.Select(Mapper.Map<Article, ArticleViewModel>).Take(12).ToList();
            if (way == 1)
            {
                return PartialView("_InTrend", model);
            }
            else
            {
                return PartialView("_Trendings", model);
            }
        }

        public ActionResult News(int page = 1, string category = null)
        {
            var articles = _articleService.GetArticles();
            var categories = _categoryService.GetCategories();
            if (!String.IsNullOrEmpty(category))
            {
                articles = articles.Where(x => x.Category.Name == category).ToList();
            }
            var mappArticles = articles.Select(Mapper.Map<Article, ArticleViewModel>).Skip((page - 1) * _pageSize).Take(_pageSize).Reverse().ToList();
            var mappCategories = categories.Select(Mapper.Map<Category, CategoryViewModel>).ToList();
            var pager = new Pager(page, articles.Count(), _pageSize);
            var model = new IndexViewModel
            {
                Articles = mappArticles,
                Pager = pager,
                Categories = mappCategories
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Find(FindArticlesViewModel model)
        {
            if (model == null)
            {
                model = new FindArticlesViewModel();
            }

            var filterModel = new ArticleFilterModel();

            filterModel.MaxDate = model.MaxDate.HasValue ? model.MaxDate.Value :
                model.MinDate.HasValue ? model.MinDate.Value : DateTime.Now;

            filterModel.MinDate = model.MinDate.HasValue ? model.MinDate.Value :
                model.MaxDate.HasValue ? model.MaxDate.Value : DateTime.Now;

            if (model.Category.HasValue)
            {
                filterModel.Categories = filterModel.Categories.Append(model.Category.Value);
            }

            var articles = _articleService.Find(filterModel, (model.Page - 1) * _pageSize, _pageSize);

            model.Articles = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(articles);

            return View(model);
        }

        [HttpGet]
        public ActionResult GetByKeyWord(string keyWord)
        {

            var articles = _articleService.GetArticlesByKeyWord(keyWord);

            var result = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(articles);

            return View(result);
        }

        [HttpPost]
        public ActionResult AutocompleteSearch(string keyWord)
        {
            var articles = _articleService.GetArticlesByKeyWord(keyWord).Take(5);

            var result = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleViewModel>>(articles);

            return PartialView(result);
        }
    }
}