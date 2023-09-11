using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.Memberships;

namespace REMCCG.Presentation.Controllers.Memberships
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
           _membershipService = membershipService;
        }
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllRecord();
            return View(memberships);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MembershipDTO membership)
        {
            if (ModelState.IsValid)
            {
                var response = await _membershipService.Create(membership);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the membership.");
            }
            return View(membership);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var membership = await _membershipService.GetRecordById(id);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MembershipDTO membership)
        {
            if (id != membership.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _membershipService.Update(membership);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the membership.");
            }
            return View(membership);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var membership = await _membershipService.GetRecordById(id);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _membershipService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

        public async Task<IActionResult> Details(int id)
        {
            var membership = await _membershipService.GetRecordById(id);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }
    }
}

        

    