using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.DTOs;
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
    public class UserService : IUserService

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;


        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IAppDbContext context,
            RoleManager<ApplicationRole> roleManager)
        {

            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _context = context ?? throw new ArgumentNullException(nameof(_context));
            _trans = _context.Begin();
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }





        public Task<ServerResponse<bool>> DeleteUserProfile(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ServerResponse<bool>> DisableUserProfile(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ServerResponse<UserDTO>> GetProfile(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserExists(string userEmail)
        {
            throw new NotImplementedException();
        }
        public async Task<ServerResponse<bool>> UpdateProfile(UpdateUser request)
        {
            var response = new ServerResponse<bool>();
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.Error = "User not found.";
                return response;
            }

            user.DOB = request.DOB;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var success = await _userManager.UpdateAsync(user);

            if (success.Succeeded)
            {
                await _trans.CommitAsync();
                response.IsSuccessful = true;
                response.SuccessMessage = "User updated successfully.";
            }
            else
            {
                await _trans.RollbackAsync();
                response.Error = "Failed to update user.";
            }

            return response;
        }

    }
}
