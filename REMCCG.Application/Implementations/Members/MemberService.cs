using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces.Members;
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

namespace REMCCG.Application.Implementations.Members
{
    public class MemberService : IMemberService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public MemberService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(MemberDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newMember = request.Adapt<Member>();

                _context.Members.Add(newMember);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Member created successfully." : "Failed to create member.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the member: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<MemberModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<MemberModel>>();

            try
            {
                var data = await _context.GetData<MemberModel>("Exec [dbo].[SP_GetMember]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<MemberModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving member: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<MemberModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<MemberModel>();

            try
            {
                var data = await _context.GetData<MemberModel>("exec [dbo].[SP_GetMemberId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the member: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(MemberDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingMember = await _context.Members.FindAsync(request.ID);

                if (existingMember == null)
                {
                    response.Data = false;
                    response.Error = "Member not found.";
                    return response;
                }

                existingMember.FirstName = request.FirstName;
                existingMember.LastName = request.LastName;
                existingMember.Email = request.Email;
                existingMember.PhoneNumber = request.PhoneNumber;
                existingMember.DOB = request.DOB;
                existingMember.DepartmentID = request.DepartmentID;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Member updated successfully." : "Failed to update member.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the member: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingMember = await _context.Members.FindAsync(id);

                if (existingMember == null)
                {
                    response.Data = false;
                    response.Error = "Member not found.";
                    return response;
                }

                _context.Members.Remove(existingMember);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Member deleted successfully." : "Failed to delete member.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the member: " + ex.Message;
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
