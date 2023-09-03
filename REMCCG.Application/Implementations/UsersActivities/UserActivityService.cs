using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.UserActivities;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.UserActivities
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IAppDbContext _context;
        public UserActivityService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ServerResponse<bool>> LogActivityAsync(UserActivityDTO request)
        {
            var response = new ServerResponse<bool>();

            if (request == null)
            {
                response.Data = false;
                response.Error = "Invalid user activity request. The request object is null.";
                return response;
            }

            try
            {
                var data = request.Adapt<UserActivity>();
                _context.UserActivities.Add(data);
                response.Data = true;
                response.SuccessMessage = "User activity logged successfully.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while logging user activity: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<IEnumerable<UserActivityViewDTO>>> FetchActivityAsync()
        {
            var response = new ServerResponse<IEnumerable<UserActivityViewDTO>>();

            try
            {
                var activities = await _context.UserActivities.ToListAsync();

                var userActivities = activities.Adapt<IEnumerable<UserActivityViewDTO>>();

                response.Data = userActivities;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.IsSuccessful = false;
                response.Error = "An error occurred while fetching user activities: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<IEnumerable<UserActivityViewDTO>>> FetchActivityByUserAsync(string userId)
        {
            var response = new ServerResponse<IEnumerable<UserActivityViewDTO>>();

            try
            {
                var activities = await _context.UserActivities
                    .Where(activity => activity.UserId == userId)
                    .ToListAsync();

                // Map UserActivity entities to UserActivityViewDTOs using Mapster
                var userActivities = activities.Adapt<IEnumerable<UserActivityViewDTO>>();

                response.Data = userActivities;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.IsSuccessful = false;
                response.Error = "An error occurred while fetching user activities by user: " + ex.Message;
            }

            return response;
        }

    }
}
