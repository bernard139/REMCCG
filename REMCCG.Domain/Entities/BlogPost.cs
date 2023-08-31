using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Blogpost
    {
        public class BlogPost
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime Date { get; set; }
            public string ImagePath { get; set; } // Path to the blog post image
            public string AuthorId { get; set; } // ApplicationUser Id for the author
            public ApplicationUser Author { get; set; } // Navigation property for the author
        }

    }
}
