using Mapster;
using Microsoft.AspNetCore.Identity;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.Roles;
using REMCCG.Domain.Entities;

namespace REMCCG.Application.Implementations.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IAppDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(IAppDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<ServerResponse<bool>> Create(RoleDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newRole = request.Adapt<ApplicationRole>();

                _context.Roles.Add(newRole);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Role created successfully." : "Failed to create Role.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the Role: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<RoleModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<RoleModel>>();

            try
            {
                var roles = await _context.GetData<ApplicationRole>("Exec [dbo].[SP_GetAlLRoles]");
                response.IsSuccessful = true;
                response.Data = roles?.AsQueryable().ProjectToType<RoleModel>().ToList();
            }
            catch (Exception ex)
            {
                response.Data = new List<RoleModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving roles: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<RoleModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<RoleModel>();

            try
            {
                var data = await _roleManager.FindByIdAsync(Convert.ToString(id));

                if (data != null)
                {
                    var record = data.Adapt<RoleModel>();
                    response.IsSuccessful = true;
                    response.Data = record;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Error = "Role not found.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the Role: " + ex.Message;
            }

            return response;
        }


        public async Task<ServerResponse<bool>> Update(RoleDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRole = await _context.Roles.FindAsync(request.Id);

                if (existingRole == null)
                {
                    response.Data = false;
                    response.Error = "Role not found.";
                    return response;
                }

                existingRole.IsActive = request.IsActive;
                existingRole.IsDeleted = request.IsDeleted;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Role updated successfully." : "Failed to update Role.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the Role: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRole = await _context.Roles.FindAsync(Convert.ToString(id));

                if (existingRole == null)
                {
                    response.Data = false;
                    response.Error = "Role not found.";
                    return response;
                }

                _context.Roles.Remove(existingRole);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Role deleted successfully." : "Failed to delete Role.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the Role: " + ex.Message;
            }

            return await Save(response);
        }

        private async Task<ServerResponse<bool>> Save(ServerResponse<bool> response)
        {
            try
            {
                int saveResult = await _context.SaveChangesAsync();
                if (saveResult > 0)
                {
                    response.Data = true;
                    response.SuccessMessage = "Attendance record updated successfully.";
                }
                else
                {
                    response.Data = false;
                    response.Error = "Failed to update attendance record.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while saving the attendance record: " + ex.Message;
            }

            return response;
        }
}

}
