using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.ServiceAttendances;

namespace REMCCG.Presentation.Controllers.ServiceAttendances
{
    public class ServiceAttendanceController : Controller
    {
        private readonly IServiceAttendanceService _serviceAttendanceService;

        public ServiceAttendanceController(IServiceAttendanceService serviceAttendanceService)
        {
            _serviceAttendanceService = serviceAttendanceService;
        }
            public IActionResult Index()
            {
                var serviceAttendances = _serviceAttendanceService.GetAllRecord();
                return View(serviceAttendances);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(ServiceAttendanceDTO serviceAttendance)
            {
                if (ModelState.IsValid)
                {
                    var response = await _serviceAttendanceService.Create(serviceAttendance);
                    if (response.IsSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the service attendance.");
                }
                return View(serviceAttendance);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var serviceAttendance = await _serviceAttendanceService.GetRecordById(id);
                if (serviceAttendance == null)
                {
                    return NotFound();
                }
                return View(serviceAttendance);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, ServiceAttendanceDTO serviceAttendance)
            {
                if (id != serviceAttendance.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var response = await _serviceAttendanceService.Update(serviceAttendance);
                    if (response.IsSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the service attendance.");
                }
                return View(serviceAttendance);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var serviceAttendance = await _serviceAttendanceService.GetRecordById(id);
                if (serviceAttendance == null)
                {
                    return NotFound();
                }
                return View(serviceAttendance);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var response = await _serviceAttendanceService.Delete(id);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { id = id, error = true });
            }

            public async Task<IActionResult> Details(int id)
            {
                var serviceAttendance = await _serviceAttendanceService.GetRecordById(id);
                if (serviceAttendance == null)
                {
                    return NotFound();
                }
                return View(serviceAttendance);
            }
    }

}
