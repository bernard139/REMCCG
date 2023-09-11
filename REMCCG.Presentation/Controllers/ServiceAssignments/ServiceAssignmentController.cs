using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.ServiceAssignments;

namespace REMCCG.Presentation.Controllers.ServiceAssignments
{
    public class ServiceAssignmentController : Controller
    {
        private readonly IServiceAssignmentService _serviceAssignmentService;

        public ServiceAssignmentController(IServiceAssignmentService serviceAssignmentService)
        {
            _serviceAssignmentService = serviceAssignmentService;
        }
        
            public IActionResult Index()
            {
                var serviceAssignments = _serviceAssignmentService.GetAllRecord();
                return View(serviceAssignments);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(ServiceAssignmentDTO serviceAssignment)
            {
                if (ModelState.IsValid)
                {
                    var response = await _serviceAssignmentService.Create(serviceAssignment);
                    if (response.IsSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the service assignment.");
                }
                return View(serviceAssignment);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var serviceAssignment = await _serviceAssignmentService.GetRecordById(id);
                if (serviceAssignment == null)
                {
                    return NotFound();
                }
                return View(serviceAssignment);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, ServiceAssignmentDTO serviceAssignment)
            {
                if (id != serviceAssignment.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var response = await _serviceAssignmentService.Update(serviceAssignment);
                    if (response.IsSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the service assignment.");
                }
                return View(serviceAssignment);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var serviceAssignment = await _serviceAssignmentService.GetRecordById(id);
                if (serviceAssignment == null)
                {
                    return NotFound();
                }
                return View(serviceAssignment);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var response = await _serviceAssignmentService.Delete(id);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { id = id, error = true });
            }

            public async Task<IActionResult> Details(int id)
            {
                var serviceAssignment = await _serviceAssignmentService.GetRecordById(id);
                if (serviceAssignment == null)
                {
                    return NotFound();
                }
                return View(serviceAssignment);
            }
        }
}


