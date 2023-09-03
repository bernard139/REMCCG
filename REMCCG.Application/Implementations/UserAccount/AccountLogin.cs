using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.UserAccounts;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.UserAccount
{
    public class AccountLogin : IAccountLogin
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountLogout> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppDbContext _context;

        public AccountLogin(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

        }

        public async Task<bool> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
