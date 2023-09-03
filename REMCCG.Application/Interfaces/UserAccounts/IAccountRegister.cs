using Microsoft.AspNetCore.Identity;
using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces.UserAccounts
{
    public interface IAccountRegister
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
    }

}
