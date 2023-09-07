using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.Constants.ErrorBuilds;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.AttendanceRecords;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.AttendanceRecords
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;
        public AttendanceRecordService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(AttendanceRecordDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRecord = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(p => p.MemberID == request.MemberID);

                if (existingRecord != null)
                {
                    response.Data = false;
                    response.Error = "Attendance record for this member already exists.";
                    return response;
                }

                var newRecord = request.Adapt<AttendanceRecord>();

                _context.AttendanceRecords.Add(newRecord);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Attendance record created successfully." : "Failed to create attendance record.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the attendance record: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRecord = await _context.AttendanceRecords.FindAsync(Id);

                if (existingRecord == null)
                {
                    response.Data = false;
                    response.Error = "Attendance record not found.";
                    return response;
                }

                _context.AttendanceRecords.Remove(existingRecord);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Attendance record deleted successfully." : "Failed to delete attendance record.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the attendance record: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<AttendanceRecordModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<AttendanceRecordModel>>();

            try
            {
                var data = await _context.GetData<AttendanceRecordModel>("Exec [dbo].[SP_GetAttendanceRecords]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<AttendanceRecordModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving attendance records: " + ex.Message;
            }

            return response;
        }




        public async Task<ServerResponse<AttendanceRecordModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<AttendanceRecordModel>();

            try
            {
                var data = await _context.GetData<AttendanceRecordModel>("exec [dbo].[SP_GetAttendanceRecordId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the attendance record: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(AttendanceRecordDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRecord = await _context.AttendanceRecords.FindAsync(request.Id);

                if (existingRecord == null)
                {
                    response.Data = false;
                    response.Error = "Attendance record not found.";
                    return response;
                }

                var update = request.Adapt<AttendanceRecord>();

                _context.AttendanceRecords.Add(update);

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Attendance record updated successfully." : "Failed to update attendance record.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the attendance record: " + ex.Message;
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
