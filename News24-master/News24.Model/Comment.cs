using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News24.Model
{
    public class Comment
    {
        public int CommentId { get; set; }

        public int ArticleId { get; set; }

        public string UserId { get; set; }

        public string Body { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual Article Article { get; set; }

        public virtual User User { get; set; }
    }
}
