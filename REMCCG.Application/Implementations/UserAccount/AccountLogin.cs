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
    public class AccountLogin : IAccountLogin
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IHttpContextAccessor _httpContext;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountLogout> _logger;
        private readonly ISessionsService _sessionsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly int loginCount = 0;
        private readonly IAppDbContext _context;
        private readonly IConfiguration _config;
        private readonly string _language;

        public AccountLogIn(
            IMessageProvider messageProvider,
            IHttpContextAccessor httpContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ILogger<AccountLogout> logger,
            ISessionsService sessionsService, IAppDbContext context, IConfiguration config)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionsService = sessionsService ?? throw new ArgumentNullException(nameof(sessionsService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            loginCount = _config.GetValue<int>("LoginCount:Count");
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];

        }

        public async Task<ServerResponse<string>> LogIn(string email, string password)
        {
            var response = new ServerResponse<string>();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_OBJECT_MAPPING,
                    ResponseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_OBJECT_MAPPING, _language)
                };

                return response;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                var userDto = user.Adapt<UserDto>();

                var tokens = await _tokenService.GenerateToken(userDto);

                var sessionRequest = new SessionDTO
                {
                    DateCreated = DateTime.Now,
                    Token = tokens.Token,
                    UserId = userDto.UserId,
                };

                var sessionResponse = await _sessionsService.CreateSessionAsync(sessionRequest, _language);
                if (sessionResponse != null && sessionResponse.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.Data = tokens.Token;
                }
                else
                {
                    response.Error = sessionResponse.Error;
                    return response;
                }
            }
            else
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.UNAUTHORIZED,
                    ResponseDescription = _messageProvider.GetMessage(ResponseCodes.UNAUTHORIZED, _language)
                };
            }

            return response;
        }

        public async Task<ServerResponse<bool>> CheckLoginCount(string language, string email)
        {
            var response = new ServerResponse<bool>();
            if (string.IsNullOrWhiteSpace(email))
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_PARAMETER,
                    ResponseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_PARAMETER, language)
                };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.INVALID_PARAMETER,
                    ResponseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_PARAMETER, language)
                };
            }
            bool isEqual = await _context.Users.AnyAsync(p => p.LoginCount.Equals(loginCount));
            response.Data = isEqual;
            response.IsSuccessful = true; return response;

        }
    }
}
