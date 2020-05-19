using News24.Web.ViewModels.ManageViewModels;
using News24.Web.ViewModels.StartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace News24.Web.ViewModels.ArticleViewModel
{
    public class ArticleDetailsViewModel
    {

        public int ArticleId { get; set; }

        public string Head { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public byte[] MainImage { get; set; }

        public DateTime PublicationDate { get; set; }


        public ContactInfoViewModel User { get; set; }

        public  ArticleCategoryViewModel Category { get; set; }

        public long Views { get; set; }
    }
}