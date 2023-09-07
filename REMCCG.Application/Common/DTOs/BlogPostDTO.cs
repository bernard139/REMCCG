using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class BlogPostDTO : BaseObjectDTO
    {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime Date { get; set; }
            public string ImagePath { get; set; }
            public string MemberID { get; set; }
            public Member Member { get; set; }

    }

    public class BlogPostModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
        public string MemberID { get; set; }
        public Member Member { get; set; }

    }
}
