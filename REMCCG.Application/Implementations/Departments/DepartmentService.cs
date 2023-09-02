using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.Departments;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;
        public DepartmentService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(DepartmentDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingDepartment = await _context.Departments
                    .FirstOrDefaultAsync(d => d.Name == request.Name);

                if (existingDepartment != null)
                {
                    response.Data = false;
                    response.Error = "A department with this name already exists.";
                    return response;
                }

                var newDepartment = request.Adapt<Department>();

                _context.Departments.Add(newDepartment);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Department created successfully." : "Failed to create department.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the department: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Update(DepartmentDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingDepartment = await _context.Departments.FindAsync(request.Id);

                if (existingDepartment == null)
                {
                    response.Data = false;
                    response.Error = "Department not found.";
                    return response;
                }

                // Update the existing department with the new values
                existingDepartment.Name = request.Name;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Department updated successfully." : "Failed to update department.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the department: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingDepartment = await _context.Departments.FindAsync(Id);

                if (existingDepartment == null)
                {
                    response.Data = false;
                    response.Error = "Department not found.";
                    return response;
                }

                _context.Departments.Remove(existingDepartment);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Department deleted successfully." : "Failed to delete department.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the department: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<DepartmentModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<DepartmentModel>();

            try
            {
                var data = await _context.GetData<DepartmentModel>("exec [dbo].[SP_GetDepartmentRecordId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the department: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<List<DepartmentModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<DepartmentModel>>();

            try
            {
                var data = await _context.GetData<DepartmentModel>("Exec [dbo].[SP_GetDepartmentRecords]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<DepartmentModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving departments: " + ex.Message;
            }

            return response;
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
