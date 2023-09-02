using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.UserAccount
{
    public class AccountLogout : IAccountLogout
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountLogout> _logger;
        private readonly ISessionsService _sessionsService;
        private readonly IAppDbContext _context;
        private readonly IConfiguration _config;
        private readonly string _language;
        public AccountLogout(IMessageProvider messageProvider, IHttpContextAccessor httpContext, SignInManager<ApplicationUser> signInManager, ILogger<AccountLogout> logger, ISessionsService sessionsService, IConfiguration config, UserManager<ApplicationUser> userManager, IAppDbContext context)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

            _logger = logger;
            _sessionsService = sessionsService;
            _config = config;

            _userManager = userManager;
            _context = context;
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
        }
        public async Task ClearsSession()
        {
            string cookie = string.Empty;
            var cookies = _httpContext.HttpContext.Request.Cookies;
            foreach (var ckie in cookies)
            {
                cookie = ckie.Key;
            }
            _logger.LogInformation("User has been logout successfully");
            await Task.Run(() => _httpContext.HttpContext.Response.Cookies.Delete(cookie));
        }

        public async Task<ServerResponse<bool>> LogOut(string userId)
        {

            var response = await _sessionsService.DeleteSessionAsync(userId, _language);
            if (response != null && response.IsSuccessful)
            {
                await _signInManager.SignOutAsync();
                await ClearsSession();
                response.Data = true;
                response.IsSuccessful = true;

            }
            return response;
        }

    }
}
