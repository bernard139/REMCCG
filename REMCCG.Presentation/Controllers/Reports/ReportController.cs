using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.Reports;

namespace REMCCG.Presentation.Controllers.Reports
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            var reports = _reportService.GetAllRecord();
            return View(reports);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportDTO report)
        {
            if (ModelState.IsValid)
            {
                var response = await _reportService.Create(report);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the report.");
            }
            return View(report);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var report = await _reportService.GetRecordById(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReportDTO report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _reportService.Update(report);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the report.");
            }
            return View(report);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var report = await _reportService.GetRecordById(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _reportService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

        public async Task<IActionResult> Details(int id)
        {
            var report = await _reportService.GetRecordById(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }
    }
}


        

        
    