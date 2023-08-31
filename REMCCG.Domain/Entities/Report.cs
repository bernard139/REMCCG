using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Report
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int MemberID { get; set; }
        public string ImagePath { get; set; } // Path to the associated image
    }
}

