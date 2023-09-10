using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.ImageGallerys;

namespace REMCCG.Presentation.Controllers.ImageGalleries
{
    public class ImageGalleryController : Controller
    {
        private readonly IImageGalleryService _imageGalleryService;

        public ImageGalleryController(IImageGalleryService imageGalleryService)
        {
            _imageGalleryService = imageGalleryService;
        }
        public IActionResult Index()
        {
            var galleries = _imageGalleryService.GetAllRecord();
            return View(galleries);
        }

        public async Task<IActionResult> Details(int id)
        {
            var gallery = await _imageGalleryService.GetRecordById(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImageGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                var response = await _imageGalleryService.Create(gallery);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the gallery.");
            }
            return View(gallery);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var gallery = await _imageGalleryService.GetRecordById(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ImageGalleryDTO gallery)
        {
            if (id != gallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _imageGalleryService.Update(gallery);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the gallery.");
            }
            return View(gallery);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var gallery = await _imageGalleryService.GetRecordById(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _imageGalleryService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

        
    }
}



        

    