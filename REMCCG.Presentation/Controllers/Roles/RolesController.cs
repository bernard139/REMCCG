using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.Roles;
using REMCCG.Domain.Entities;

namespace REMCCG.Presentation.Controllers.Roles
{
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
    

            public IActionResult Index()
            {
                var roles = _roleService.GetAllRecord();
                return View(roles);
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(RoleDTO role)
            {
                if (ModelState.IsValid)
                {
                    var response = await _roleService.Create(role);
                    if (response.IsSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the role.");
                }
                return View(role);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var role = await _roleService.GetRecordById(id);
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, RoleDTO role)
            {
                if (id == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var response = await _roleService.Update(role);
                    if (response.IsSuccessful)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the role.");
                }
                return View(role);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var role = await _roleService.GetRecordById(id);
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var response = await _roleService.Delete(id);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { id = id, error = true });
            }

            public async Task<IActionResult> Details(int id)
            {
                var role = await _roleService.GetRecordById(id);
                if (role == null)
                {
                    return NotFound();
                }
                return View(role);
            }
    }
}

        

        
   