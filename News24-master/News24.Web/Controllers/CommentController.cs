using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using News24.Data.Identity;
using News24.Model;
using News24.Service;

namespace News24.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ApplicationUserManager _applicationUserManager;

        public CommentController(IArticleService articleService, ICommentService commentService, ApplicationUserManager applicationUserManager)
        {
            _articleService = articleService;
            _commentService = commentService;
            _applicationUserManager = applicationUserManager;
        }

        [HttpPost]
        public ActionResult Create(string commentBody, int articleId)
        {
            var article = _articleService.GetArticle(articleId);
            if (article == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (string.IsNullOrEmpty(commentBody))
            {
                RedirectToAction("Details", "Start", new { id = articleId });
            }

            var user = _applicationUserManager.FindById(User.Identity.GetUserId());
            if (user.LockoutEndDateUtc > DateTime.Now)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var comment = new Comment
            {
                ArticleId = article.ArticleId,
                Body = commentBody,
                CommentDate = DateTime.Now,
                UserId = User.Identity.GetUserId()
            };

            _commentService.CreateComment(comment);
            Logger.Log.Info($"{User.Identity.Name} оставил комментарий к статье {articleId}. Содержание комментария: \"{commentBody}\"");
            return RedirectToAction("Details", "Start", new { id = articleId });
        }
    }
}