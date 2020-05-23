using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace News24.Web.Areas.Admin.ViewModels.ArticleViewModels
{
    public class ArticleViewModel
    {

        public int ArticleId { get; set; }

        [Display(Name = "Заголовок")]
        [Required(ErrorMessage = "Обязательное поле")]
        [MaxLength(200, ErrorMessage = "Вы ввели слишком много символов!")]
        [MinLength(2, ErrorMessage = "Вы ввели слишком мало символов!")]
        public string Head { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [AllowHtml]
        [Display(Name = "Содержимое")]
        [MaxLength(100000, ErrorMessage = "Вы ввели слишком много символов!")]
        [MinLength(2, ErrorMessage = "Вы ввели слишком мало символов!")]
        public string Body { get; set; }

        public ArticleCategoryViewModel Category { get; set; }

        [Display(Name = "Дата публикации")]
        public DateTime PublicationDate { get; set; }

        public byte[] MainImage { get; set; }

    }
}