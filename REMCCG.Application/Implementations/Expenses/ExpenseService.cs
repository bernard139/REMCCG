using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.Expenses;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Common.DTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;
using REMCCG.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace REMCCG.Application.Implementations.Expenses
{
    public class ExpenseService : IExpenseService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;
        public ExpenseService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(ExpenseDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingExpense = await _context.Expenses
                    .FirstOrDefaultAsync(e => e.Description == request.Description);

                if (existingExpense != null)
                {
                    response.Data = false;
                    response.Error = "An expense with this name already exists.";
                    return response;
                }

                var newExpense = request.Adapt<Expense>();

                _context.Expenses.Add(newExpense);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Expenses created successfully." : "Failed to create expenses.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the expenses: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Update(ExpenseDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingExpense = await _context.Expenses.FindAsync(request.Id);

                if (existingExpense == null)
                {
                    response.Data = false;
                    response.Error = "Expenses not found.";
                    return response;
                }

                existingExpense.Description = request.Description;
                existingExpense.Amount = request.Amount;
                existingExpense.Date = request.Date;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Expenses updated successfully." : "Failed to update expenses.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the expenses: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingExpense = await _context.Expenses.FindAsync(Id);

                if (existingExpense == null)
                {
                    response.Data = false;
                    response.Error = "Expenses not found.";
                    return response;
                }

                _context.Expenses.Remove(existingExpense);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Expenses deleted successfully." : "Failed to delete expense.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the expenses: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<ExpenseModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<ExpenseModel>>();

            try
            {
                var data = await _context.GetData<ExpenseModel>("Exec [dbo].[SP_GetExpenses]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ExpenseModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving expenses: " + ex.Message;
            }

            return response;
        }




        public async Task<ServerResponse<ExpenseModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<ExpenseModel>();

            try
            {
                var data = await _context.GetData<ExpenseModel>("exec [dbo].[SP_GetExpensesId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the expenses: " + ex.Message;
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
                    response.SuccessMessage = "Expenses updated successfully.";
                }
                else
                {
                    response.Data = false;
                    response.Error = "Failed to update expenses.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while saving the expenses: " + ex.Message;
            }

            return response;
        }
    }
}
