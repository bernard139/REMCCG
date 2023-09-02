using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces.UserAccounts
{
    public interface IUserService
    {

        Task<bool> IsUserExists(string userEmail);
        Task<ServerResponse<bool>> DeleteUserProfile(string email);
        Task<ServerResponse<bool>> DisableUserProfile(string email);
        Task<ServerResponse<UserDTO>> GetProfile(string userId);
        Task<ServerResponse<bool>> UpdateProfile(UpdateUser request);


    }
}
