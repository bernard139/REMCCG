using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    [Keyless]
    public class UserActivity
    {
        public string UserId { get; set; }
        public bool IsActivityResult { get; set; }
        public string Activity { get; set; }
        //public long? CountryId { get; set; }
        public string Action { get; set; }
    }
}
