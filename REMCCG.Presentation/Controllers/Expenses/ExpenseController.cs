using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.Expenses;

namespace REMCCG.Presentation.Controllers.Expenses
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
        public IActionResult Index()
        {
            var expenses = _expenseService.GetAllRecord(); // Define this method in your service
            return View(expenses);
        }


        public async Task<IActionResult> Details(int id)
        {
            var expense = await _expenseService.GetRecordById(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseDTO expense)
        {
            if (ModelState.IsValid)
            {
                var response = await _expenseService.Create(expense);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the expense.");
            }
            return View(expense);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetRecordById(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ExpenseDTO expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _expenseService.Update(expense);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the expense.");
            }
            return View(expense);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetRecordById(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _expenseService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

    }
}



        
