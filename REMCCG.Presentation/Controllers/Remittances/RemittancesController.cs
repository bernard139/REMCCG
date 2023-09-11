using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Implementations.AttendanceRecords;
using REMCCG.Application.Interfaces.AttendanceRecords;
using REMCCG.Application.Interfaces.Remittances;

namespace REMCCG.Presentation.Controllers.Remittances
{
    public class RemittancesController : Controller
    {
        private readonly IRemittanceService _remittanceService;

        public RemittancesController(IRemittanceService remittanceService)
        {
            _remittanceService = remittanceService;
        }
        public IActionResult Index()
        {
            // Get remittance history from the service
            var remittanceHistory = _remittanceService.GetAllRecord(); // Define this method in your service

            return View(remittanceHistory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RemittanceDTO record)
        {
            if (ModelState.IsValid)
            {
                var response = await _remittanceService.Create(record);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the remittance.");
            }
            return View(record);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var record = await _remittanceService.GetRecordById(id);
            if (record == null)
            {
                return NotFound();
            }
            return View(record);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RemittanceDTO record)
        {
            if (id != record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _remittanceService.Update(record);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the record.");
            }
            return View(record);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var record = await _remittanceService.GetRecordById(id);
            if (record == null)
            {
                return NotFound();
            }
            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _remittanceService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }
    }
}












