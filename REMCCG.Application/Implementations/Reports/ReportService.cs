using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces.Reports;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using Mapster;
using Microsoft.Data.SqlClient;

namespace REMCCG.Application.Implementations.Reports
{
    public class ReportService : IReportService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public ReportService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(ReportDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newReport = request.Adapt<Report>();

                _context.Reports.Add(newReport);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Report created successfully." : "Failed to create report.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the report: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<ReportModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<ReportModel>>();

            try
            {
                var data = await _context.GetData<ReportModel>("Exec [dbo].[SP_GetAllReport]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ReportModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving report: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<ReportModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<ReportModel>();

            try
            {
                var data = await _context.GetData<ReportModel>("exec [dbo].[SP_GetReportId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the report: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(ReportDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingReport = await _context.Reports.FindAsync(request.ID);

                if (existingReport == null)
                {
                    response.Data = false;
                    response.Error = "Report not found.";
                    return response;
                }

                existingReport.Title = request.Title;
                existingReport.Content = request.Content;
                existingReport.Date = request.Date;
                existingReport.MemberID = request.MemberID;
                existingReport.ImagePath = request.ImagePath;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Report updated successfully." : "Failed to update report.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the report: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingReport = await _context.Reports.FindAsync(id);

                if (existingReport == null)
                {
                    response.Data = false;
                    response.Error = "Report not found.";
                    return response;
                }

                _context.Reports.Remove(existingReport);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Report deleted successfully." : "Failed to delete report.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the report: " + ex.Message;
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
