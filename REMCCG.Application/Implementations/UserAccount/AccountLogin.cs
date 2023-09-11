using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.UserAccounts;
using REMCCG.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.UserAccount
{
    public class AccountLogin : IAccountLogin
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountLogin(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);
                    _httpContextAccessor.HttpContext.Session.SetString("Email", user.Email);

                    return true;
                }
            }
            return false;
        }
    }
}
