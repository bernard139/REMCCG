using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class UserActivityDTO
    {
        public string UserId { get; set; }
        public bool IsActivityResult { get; set; }
        public string Activity { get; set; }
        //public long? CountryId { get; set; }
        public string Action { get; set; }
    }

    public class UserActivityViewDTO
    {
        public string UserId { get; set; }
        public bool IsActivityResult { get; set; }
        public string Activity { get; set; }
        //public long? CountryId { get; set; }
        public string Action { get; set; }
    }
}
