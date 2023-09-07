using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.BlogPosts;
using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REMCCG.Application.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public BlogPostService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<List<BlogPostModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<BlogPostModel>>();

            try
            {
                var data = await _context.GetData<BlogPostModel>("Exec [dbo].[SP_GetBlogPosts]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.IsSuccessful = false;
                response.Error = "An error occurred while fetching blog posts: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<BlogPostModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<BlogPostModel>();

            try
            {
                var data = await _context.GetData<BlogPostModel>("exec [dbo].[SP_GetBlogPostId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while fetching the blog post: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Create(BlogPostDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingPost = await _context.BlogPosts.FirstOrDefaultAsync(p => p.Title == request.Title);

                if (existingPost != null)
                {
                    response.Data = false;
                    response.Error = "A blog post with this title already exists.";
                }
                else
                {
                    var newPost = request.Adapt<Blogpost>();
                    _context.BlogPosts.Add(newPost);
                    int saveResult = await _context.SaveChangesAsync();

                    response.Data = saveResult > 0;
                    response.Error = response.Data ? "Blog post created successfully." : "Failed to create blog post.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the blog post: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Update(BlogPostDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingPost = await _context.BlogPosts.FindAsync(request.Id);

                if (existingPost == null)
                {
                    response.Data = false;
                    response.Error = "Blog post not found.";
                }
                else
                {

                    var update = request.Adapt<Blogpost>();
                    _context.BlogPosts.Add(update);

                    int updateResult = await _context.SaveChangesAsync();

                    response.Data = updateResult > 0;
                    response.Error = response.Data ? "Blog post updated successfully." : "Failed to update blog post.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the blog post: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingRecord = await _context.BlogPosts.FindAsync(Id);

                if (existingRecord == null)
                {
                    response.Data = false;
                    response.Error = "Blog Post not found.";
                }
                else
                {
                    _context.BlogPosts.Remove(existingRecord);
                    int deleteResult = await _context.SaveChangesAsync();

                    response.Data = deleteResult > 0;
                    response.Error = response.Data ? "Blog Post deleted successfully." : "Failed to delete blog post.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the blog post: " + ex.Message;
            }

            return await Save(response);
        }


        private async Task<ServerResponse<bool>> Save(ServerResponse<bool> response)
        {
            try
            {
                int save = await _context.SaveChangesAsync();
                if (save > 0)
                {
                    response.Data = true;
                    response.SuccessMessage = "Blog Post updated successfully.";
                }
                else
                {
                    response.Data = false;
                    response.Error = "Failed to update Blog Post.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while saving changes: " + ex.Message;
            }

            return response;
        }
    }

}
