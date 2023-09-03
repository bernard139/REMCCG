using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces.UserActivities
{
    public interface IUserActivityService
    {
        public Task<ServerResponse<bool>> LogActivityAsync(UserActivityDTO request);
        public Task<ServerResponse<IEnumerable<UserActivityViewDTO>>> FetchActivityAsync();
        public Task<ServerResponse<IEnumerable<UserActivityViewDTO>>> FetchActivityByUserAsync(string userId);
    }
}
