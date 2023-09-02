using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class DepartmentDTO : BaseObjectDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }
    }

    public class DepartmentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }
    }
}
