using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class Department
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }
    }
}
