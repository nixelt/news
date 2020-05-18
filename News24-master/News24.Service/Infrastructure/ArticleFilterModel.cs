using News24.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace News24.Service.Infrastructure
{
    public class ArticleFilterModel
    {
        public DateTime MinDate { get; set; } = DateTime.Now;

        public DateTime MaxDate { get; set; } = DateTime.Now;

        public IEnumerable<int> Categories { get; set; } = new List<int>();

        public Expression<Func<Article, bool>> GetExpression()
        {
            return x => x.PublicationDate >= MinDate && x.PublicationDate < MaxDate && Categories.Contains(x.CategoryId);
        }
    }
}
