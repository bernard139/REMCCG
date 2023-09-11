using Microsoft.AspNetCore.Mvc;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Interfaces.BlogPosts;

namespace REMCCG.Presentation.Controllers.BlogPosts
{
    public class BlogPostController : Controller
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }
        public IActionResult Index()
        {
            var blogPosts = _blogPostService.GetAllRecord(); // Define this method in your service
            return View(blogPosts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _blogPostService.GetRecordById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogPostDTO post)
        {
            if (ModelState.IsValid)
            {
                var response = await _blogPostService.Create(post);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the post.");
            }
            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _blogPostService.GetRecordById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogPostDTO post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _blogPostService.Update(post);
                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while updating the post.");
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _blogPostService.GetRecordById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _blogPostService.Delete(id);
            if (response.IsSuccessful)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete", new { id = id, error = true });
        }
    }
}

        

        
    