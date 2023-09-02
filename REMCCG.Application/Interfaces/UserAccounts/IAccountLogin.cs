using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces.UserAccounts
{
    public interface IAccountLogin
    {
        Task<ServerResponse<string>> LogIn(string email, string password);
        Task<ServerResponse<bool>> CheckLoginCount(string language, string email);
    }
}
