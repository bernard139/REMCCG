using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.FunctionalDepartments;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.FunctionalDepartments
{
    public class FunctionalDepartmentService : IFunctionalDepartmentService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public FunctionalDepartmentService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(FunctionalDepartmentDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingDepartment = await _context.FunctionalDepartments
                    .FirstOrDefaultAsync(d => d.Name == request.Name);

                if (existingDepartment != null)
                {
                    response.Data = false;
                    response.Error = "A functional department with this name already exists.";
                    return response;
                }

                var newDepartment = request.Adapt<FunctionalDepartment>();

                _context.FunctionalDepartments.Add(newDepartment);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Functional department created successfully." : "Failed to create functional department.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the functional department: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Update(FunctionalDepartmentDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingDepartment = await _context.FunctionalDepartments.FindAsync(request.Id);

                if (existingDepartment == null)
                {
                    response.Data = false;
                    response.Error = "Functional department not found.";
                    return response;
                }

                existingDepartment.Name = request.Name;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Functional department updated successfully." : "Failed to update functional department.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the functional department: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingDepartment = await _context.FunctionalDepartments.FindAsync(Id);

                if (existingDepartment == null)
                {
                    response.Data = false;
                    response.Error = "Functional department not found.";
                    return response;
                }

                _context.FunctionalDepartments.Remove(existingDepartment);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Functional department deleted successfully." : "Failed to delete functional department.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the functional department: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<FunctionalDepartmentModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<FunctionalDepartmentModel>>();

            try
            {
                var data = await _context.GetData<FunctionalDepartmentModel>("Exec [dbo].[SP_GetFunctionalDepartment]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<FunctionalDepartmentModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving functional department: " + ex.Message;
            }

            return response;
        }




        public async Task<ServerResponse<FunctionalDepartmentModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<FunctionalDepartmentModel>();

            try
            {
                var data = await _context.GetData<FunctionalDepartmentModel>("exec [dbo].[SP_GetFunctionalDepartmentId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the functional department: " + ex.Message;
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
                    response.SuccessMessage = "functional department updated successfully.";
                }
                else
                {
                    response.Data = false;
                    response.Error = "Failed to update functional department.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while saving the functional department: " + ex.Message;
            }

            return response;
        }
    }
}
