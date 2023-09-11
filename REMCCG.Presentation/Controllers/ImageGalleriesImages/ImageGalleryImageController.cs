using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.ImageGalleryImages;

namespace REMCCG.Presentation.Controllers.ImageGallariesImages
{
    public class ImageGalleryImageController : Controller
    {
        private readonly IImageGalleryImageService _imageGalleryImageService;

        public ImageGalleryImageController(IImageGalleryImageService imageGalleryImageService)
        {
            _imageGalleryImageService = imageGalleryImageService;
        }
        public IActionResult Index()
        {
            var galleryImages = _imageGalleryImageService.GetAllRecord();
            return View(galleryImages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ImageGalleryImageDTO image)
        {
            if (ModelState.IsValid)
            {
                var response = await _imageGalleryImageService.Create(image);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the image.");
            }
            return View(image);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var image = await _imageGalleryImageService.GetRecordById(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ImageGalleryImageDTO image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _imageGalleryImageService.Update(image);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the image.");
            }
            return View(image);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var image = await _imageGalleryImageService.GetRecordById(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _imageGalleryImageService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }

        public async Task<IActionResult> Details(int id)
        {
            var image = await _imageGalleryImageService.GetRecordById(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }
    }
}


        

        
    