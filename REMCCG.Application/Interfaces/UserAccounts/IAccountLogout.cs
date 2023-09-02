using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces.UserAccounts
{
    public interface IAccountLogout
    {
        Task<ServerResponse<bool>> LogOut(string userId);
        Task ClearsSession();

    }
}
