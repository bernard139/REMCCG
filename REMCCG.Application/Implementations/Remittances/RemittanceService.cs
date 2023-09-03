using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces.Remittances;
using REMCCG.Application.Interfaces;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.Data.SqlClient;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace REMCCG.Application.Implementations.Remittances
{
    public class RemittanceService : IRemittanceService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public RemittanceService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(RemittanceDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newRemittance = request.Adapt<Remittance>();

                _context.Remittances.Add(newRemittance);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Remittance created successfully." : "Failed to create remittance.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the remittance: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<RemittanceModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<RemittanceModel>>();

            try
            {
                var data = await _context.GetData<RemittanceModel>("Exec [dbo].[SP_GetRemittance]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<RemittanceModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving remittance: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<RemittanceModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<RemittanceModel>();

            try
            {
                var data = await _context.GetData<RemittanceModel>("exec [dbo].[SP_GetRemittanceId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the remittance: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(RemittanceDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRemittance = await _context.Remittances.FindAsync(request.ID);

                if (existingRemittance == null)
                {
                    response.Data = false;
                    response.Error = "Remittance not found.";
                    return response;
                }

                existingRemittance.RemittanceType = request.RemittanceType;
                existingRemittance.Amount = request.Amount;
                existingRemittance.Date = request.Date;
                existingRemittance.MemberID = request.MemberID;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Remittance updated successfully." : "Failed to update remittance.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the remittance: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRemittance = await _context.Remittances.FindAsync(id);

                if (existingRemittance == null)
                {
                    response.Data = false;
                    response.Error = "Remittance not found.";
                    return response;
                }

                _context.Remittances.Remove(existingRemittance);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Remittance deleted successfully." : "Failed to delete remittance.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the remittance: " + ex.Message;
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
