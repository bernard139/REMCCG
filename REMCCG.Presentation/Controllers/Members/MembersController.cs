using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.Members;

namespace REMCCG.Presentation.Controllers.Members
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var members = _memberService.GetAllRecord();
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberDTO member)
        {
            if (ModelState.IsValid)
            {
                var response = await _memberService.Create(member);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the member.");
            }
            return View(member);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberService.GetRecordById(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MemberDTO member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _memberService.Update(member);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the member.");
            }
            return View(member);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberService.GetRecordById(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _memberService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

        public async Task<IActionResult> Details(int id)
        {
            var member = await _memberService.GetRecordById(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }
    }
}


        

   
