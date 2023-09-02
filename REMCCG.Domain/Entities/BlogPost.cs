using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Blogpost
    {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime Date { get; set; }
            public string ImagePath { get; set; }
            public string AuthorId { get; set; }
            public ApplicationUser Author { get; set; }

    }
}
