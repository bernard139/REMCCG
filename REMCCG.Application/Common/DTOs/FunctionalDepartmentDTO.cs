using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class FunctionalDepartmentDTO : BaseObjectDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class FunctionalDepartmentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
