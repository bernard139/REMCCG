using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.Departments;

namespace REMCCG.Presentation.Controllers.Departments
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var departments = _departmentService.GetAllRecord(); // Define this method in your service
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentDTO department)
        {
            if (ModelState.IsValid)
            {
                var response = await _departmentService.Create(department);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the department.");
            }
            return View(department);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetRecordById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DepartmentDTO department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _departmentService.Update(department);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the department.");
            }
            return View(department);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetRecordById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _departmentService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }
    }

}

        

        
   