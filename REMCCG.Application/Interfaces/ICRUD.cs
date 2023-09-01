using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces
{
    public interface ICRUD<Request, Response>
    {
        Task<ServerResponse<bool>> Create(Request request);
        Task<ServerResponse<bool>> Update(Request request);
        Task<ServerResponse<bool>> Delete(object id);
        Task<ServerResponse<List<Response>>> GetAllRecord();
        Task<ServerResponse<Response>> GetRecordById(object id);


    }
}
