using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.FunctionalDepartments;

namespace REMCCG.Presentation.Controllers.FunctionalDepartments
{
    public class FunctionalDepartmentController : Controller
    {
        private readonly IFunctionalDepartmentService _functionalDepartmentService;

        public FunctionalDepartmentController(IFunctionalDepartmentService functionalDepartmentService)
        {
            _functionalDepartmentService = functionalDepartmentService;
        }
        public IActionResult Index()
        {
            var functionalDepartments = _functionalDepartmentService.GetAllRecord();
            return View(functionalDepartments);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FunctionalDepartmentDTO department)
        {
            if (ModelState.IsValid)
            {
                var response = await _functionalDepartmentService.Create(department);
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
            var department = await _functionalDepartmentService.GetRecordById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, FunctionalDepartmentDTO department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _functionalDepartmentService.Update(department);
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
            var department = await _functionalDepartmentService.GetRecordById(id);
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
            var response = await _functionalDepartmentService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

    }
}


        

        
  
