using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces.ServiceAssignments;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using Mapster;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace REMCCG.Application.Implementations.ServiceAssignments
{
    public class ServiceAssignmentService : IServiceAssignmentService
    {
        private readonly IAppDbContext _context;

        public ServiceAssignmentService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ServerResponse<bool>> Create(ServiceAssignmentDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newServiceAssignment = request.Adapt<ServiceAssignment>();

                _context.ServiceAssignments.Add(newServiceAssignment);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Service Assignment created successfully." : "Failed to create Service Assignment.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the Service Assignment: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<ServiceAssignmentModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<ServiceAssignmentModel>>();

            try
            {
                var data = await _context.GetData<ServiceAssignmentModel>("Exec [dbo].[SP_GetServiceAssignment]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ServiceAssignmentModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving service assignment " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<ServiceAssignmentModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<ServiceAssignmentModel>();

            try
            {
                var data = await _context.GetData<ServiceAssignmentModel>("exec [dbo].[SP_GetServiceAssignmentId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the service assignment: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(ServiceAssignmentDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingServiceAssignment = await _context.ServiceAssignments.FindAsync(request.ID);

                if (existingServiceAssignment == null)
                {
                    response.Data = false;
                    response.Error = "Service Assignment not found.";
                    return response;
                }

                // Update the existing Service Assignment with the new values
                existingServiceAssignment.AttendanceEventID = request.AttendanceEventID;
                existingServiceAssignment.LeaderId = request.LeaderId;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Service Assignment updated successfully." : "Failed to update Service Assignment.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the Service Assignment: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingServiceAssignment = await _context.ServiceAssignments.FindAsync(id);

                if (existingServiceAssignment == null)
                {
                    response.Data = false;
                    response.Error = "Service Assignment not found.";
                    return response;
                }

                _context.ServiceAssignments.Remove(existingServiceAssignment);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Service Assignment deleted successfully." : "Failed to delete Service Assignment.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the Service Assignment: " + ex.Message;
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
