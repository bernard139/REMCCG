using Azure;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.Memberships;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Implementations.Memberships
{
    public class MembershipService : IMembershipService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;
        public MembershipService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }
            public async Task<ServerResponse<bool>> Create(MembershipDTO request)
            {
                var response = new ServerResponse<bool>();

                try
                {
                    var newMembership = request.Adapt<Membership>();

                    _context.Memberships.Add(newMembership);
                    int saveResult = await _context.SaveChangesAsync();

                    response.Data = saveResult > 0;
                    response.Error = response.Data ? "Membership created successfully." : "Failed to create membership.";
                }
                catch (Exception ex)
                {
                    response.Data = false;
                    response.Error = "An error occurred while creating the membership: " + ex.Message;
                }

            return await Save(response);
        }

            public async Task<ServerResponse<List<MembershipModel>>> GetAllRecord()
            {
                var response = new ServerResponse<List<MembershipModel>>();

                try
                {
                var data = await _context.GetData<MembershipModel>("Exec [dbo].[SP_GetMembership]");
                    response.Data = data;
                    response.IsSuccessful = true;
                }
                catch (Exception ex)
                {
                    response.Data = new List<MembershipModel>();
                    response.IsSuccessful = false;
                    response.Error = "An error occurred while retrieving membership: " + ex.Message;
                }

                return response;
            }

            public async Task<ServerResponse<MembershipModel>> GetRecordById(object id)
            {
                    var response = new ServerResponse<MembershipModel>();

                try
                {
                    var data = await _context.GetData<MembershipModel>("exec [dbo].[SP_GetMembershipId]", new SqlParameter("@id", id));

                    response.IsSuccessful = true;
                    response.Data = data?.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    response.IsSuccessful = false;
                    response.Error = "An error occurred while retrieving the membership: " + ex.Message;
                }

                return response;
            }

            public async Task<ServerResponse<bool>> Update(MembershipDTO request)
            {
                var response = new ServerResponse<bool>();

                try
                {
                    var existingMembership = await _context.Memberships.FindAsync(request.ID);

                    if (existingMembership == null)
                    {
                        response.Data = false;
                        response.Error = "Membership not found.";
                        return response;
                    }

                    existingMembership.MemberID = request.MemberID;
                    existingMembership.DateJoined = request.DateJoined;

                    int updateResult = await _context.SaveChangesAsync();

                    response.Data = updateResult > 0;
                    response.Error = response.Data ? "Membership updated successfully." : "Failed to update membership.";
                }
                catch (Exception ex)
                {
                    response.Data = false;
                    response.Error = "An error occurred while updating the membership: " + ex.Message;
                }

            return await Save(response);
        }

            public async Task<ServerResponse<bool>> Delete(object id)
            {
                var response = new ServerResponse<bool>();

                try
                {
                    var existingMembership = await _context.Memberships.FindAsync(id);

                    if (existingMembership == null)
                    {
                        response.Data = false;
                        response.Error = "Membership not found.";
                        return response;
                    }

                    _context.Memberships.Remove(existingMembership);
                    int deleteResult = await _context.SaveChangesAsync();

                    response.Data = deleteResult > 0;
                    response.Error = response.Data ? "Membership deleted successfully." : "Failed to delete membership.";
                }
                catch (Exception ex)
                {
                    response.Data = false;
                    response.Error = "An error occurred while deleting the membership: " + ex.Message;
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
                response.SuccessMessage = "Membership updated successfully.";
            }
            else
            {
                response.Data = false;
                response.Error = "Failed to update membership.";
            }
        }
        catch (Exception ex)
        {
            response.Data = false;
            response.Error = "An error occurred while saving the membership: " + ex.Message;
        }

        return response;
    }


}
}
