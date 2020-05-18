
using News24.Web.ViewModels.StartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News24.Web.Models
{
    public class FindArticlesViewModel
    {
        public int Page { get; set; } = 1;

        public int? Category { get; set; } 

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }

        public IEnumerable<ArticleViewModel> Articles { get; set; }
    }
}