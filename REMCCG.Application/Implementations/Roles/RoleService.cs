using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.Roles
{
    public class RoleService : ResponseBaseService, IRoleService
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IAppDbContext _context;
        private readonly string _language;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IDbContextTransaction _trans;
        public RoleService(IMessageProvider messageProvider, IHttpContextAccessor httpContext, RoleManager<ApplicationRole> roleManager, IAppDbContext context) : base(messageProvider)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();

        }

        public async Task<ServerResponse<bool>> Create(RoleDTO request)
        {
            var response = new ServerResponse<bool>();
            if (request.IsValid(out ValidationResponse source, _messageProvider, _language))
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language);

            }
            IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole { Name = request.RoleName, IsActive = true });
            if (result.Succeeded)
            {

                await _trans.CommitAsync();
                response.IsSuccessful = true;

                SetSuccess(response, true, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                await _trans.RollbackAsync();
                SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, _language);
            }
            return response;

        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            string reqId = Convert.ToString(id);
            var response = new ServerResponse<bool>();
            if (string.IsNullOrWhiteSpace(reqId))
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language);

            }
            var result = await _roleManager.FindByIdAsync(reqId);
            if (result is null)
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language);
            }
            result.IsDeleted = true;
            _context.Roles.Update(result);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                await _trans.CommitAsync();
                response.IsSuccessful = true;
                SetSuccess(response, true, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                await _trans.RollbackAsync();
                SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, _language);
            }
            return response;

        }

        public async Task<ServerResponse<List<RoleModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<RoleModel>>();
            var roles = await _context.GetData<ApplicationRole>("Exec [dbo].[SP_GetAlLRoles]");
            response.IsSuccessful = true;
            response.Data = roles?.AsQueryable().ProjectToType<RoleModel>().ToList();
            return response;

        }

        public async Task<ServerResponse<RoleModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<RoleModel>();
            var data = await _roleManager.FindByIdAsync(Convert.ToString(id));
            var record = data?.Adapt<RoleModel>();
            response.IsSuccessful = true;
            response.Data = record;
            return response;

        }



        public async Task<ServerResponse<bool>> Update(RoleDTO request)
        {

            var response = new ServerResponse<bool>();
            if (!request.IsValid(out ValidationResponse source, _messageProvider, _language))
            {
                return SetError(response, source.Code, source.Message, _language);

            }
            var result = await _roleManager.FindByIdAsync(request.Id);
            if (result is null)
            {
                return SetError(response, ResponseCodes.INVALID_PARAMETER, _language);
            }
            result.Name = request.RoleName;
            var identityResult = await _roleManager.UpdateAsync(result);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                await _trans.CommitAsync();
                response.IsSuccessful = true;
                SetSuccess(response, true, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                await _trans.RollbackAsync();
                SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, _language);
            }
            return response;
        }
    }
}
