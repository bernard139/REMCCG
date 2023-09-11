using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.AttendanceRecords;

namespace REMCCG.Presentation.Controllers.AttendanceRecords
{
    public class AttendanceRecordController : Controller
    {
        private readonly IAttendanceRecordService _attendanceRecordService;

        public AttendanceRecordController(IAttendanceRecordService attendanceRecordService)
        {
            _attendanceRecordService = attendanceRecordService;
        }
        public IActionResult Index()
        {
            var attendanceRecords = _attendanceRecordService.GetAllRecord(); // Define this method in your service
            return View(attendanceRecords);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendanceRecordDTO record)
        {
            if (ModelState.IsValid)
            {
                var response = await _attendanceRecordService.Create(record);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the record.");
            }
            return View(record);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var record = await _attendanceRecordService.GetRecordById(id);
            if (record == null)
            {
                return NotFound();
            }
            return View(record);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AttendanceRecordDTO record)
        {
            if (id != record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _attendanceRecordService.Update(record);
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
            var record = await _attendanceRecordService.GetRecordById(id);
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
            var response = await _attendanceRecordService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

    }
}


    

        

        
        
 
