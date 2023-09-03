using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class RoleDTO
    {
        public string RoleName { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
    public class RoleModel : BaseObjectModel
    {
        public string RoleName { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
