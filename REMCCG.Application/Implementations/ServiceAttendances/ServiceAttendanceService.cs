using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces.ServiceAttendances;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using Mapster;
using Microsoft.Data.SqlClient;

namespace REMCCG.Application.Implementations.ServiceAttendances
{
    public class ServiceAttendanceService : IServiceAttendanceService
    {
        private readonly IAppDbContext _context;

        public ServiceAttendanceService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ServerResponse<bool>> Create(ServiceAttendanceDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newServiceAttendance = request.Adapt<ServiceAttendance>();

                _context.ServiceAttendances.Add(newServiceAttendance);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Service Attendance created successfully." : "Failed to create Service Attendance.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the Service Attendance: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<ServiceAttendanceModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<ServiceAttendanceModel>>();

            try
            {
                var data = await _context.GetData<ServiceAttendanceModel>("Exec [dbo].[SP_GetServiceAttendance]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ServiceAttendanceModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving service attendance: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<ServiceAttendanceModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<ServiceAttendanceModel>();

            try
            {
                var data = await _context.GetData<ServiceAttendanceModel>("exec [dbo].[SP_GetServiceAttendanceId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the service attendance: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(ServiceAttendanceDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingServiceAttendance = await _context.ServiceAttendances.FindAsync(request.ID);

                if (existingServiceAttendance == null)
                {
                    response.Data = false;
                    response.Error = "Service Attendance not found.";
                    return response;
                }


                var updateServiceAttendance = request.Adapt<ServiceAttendance>();

                _context.ServiceAttendances.Add(updateServiceAttendance);

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Service Attendance updated successfully." : "Failed to update Service Attendance.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the Service Attendance: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingServiceAttendance = await _context.ServiceAttendances.FindAsync(id);

                if (existingServiceAttendance == null)
                {
                    response.Data = false;
                    response.Error = "Service Attendance not found.";
                    return response;
                }

                _context.ServiceAttendances.Remove(existingServiceAttendance);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Service Attendance deleted successfully." : "Failed to delete Service Attendance.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the Service Attendance: " + ex.Message;
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
                    response.SuccessMessage = "Service attendance updated successfully.";
                }
                else
                {
                    response.Data = false;
                    response.Error = "Failed to update service attendance.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while saving the service attendance: " + ex.Message;
            }

            return response;   
        }
    }

}
