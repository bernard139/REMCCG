using Microsoft.AspNetCore.Http;
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
        Task<bool> LoginAsync(LoginRequest model);
    }
}
