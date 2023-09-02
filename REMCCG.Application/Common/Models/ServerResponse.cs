using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.Models
{
    public class ServerResponse<T>
    {
        public ServerResponse()
        {
            IsSuccessful = true;
        }

        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
        public string SuccessMessage { get; set; }
    }
}

