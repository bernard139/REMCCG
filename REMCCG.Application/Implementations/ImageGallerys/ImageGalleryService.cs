using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.ImageGallerys;
using REMCCG.Domain.Entities;

namespace REMCCG.Application.Implementations.ImageGallerys
{
    public class ImageGalleryService : IImageGalleryService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public ImageGalleryService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(ImageGalleryDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingGallery = await _context.ImageGalleries
                    .FirstOrDefaultAsync(g => g.Title == request.Title);

                if (existingGallery != null)
                {
                    response.Data = false;
                    response.Error = "An image gallery with this title already exists.";
                    return response;
                }

                var newGallery = request.Adapt<ImageGallery>();

                _context.ImageGalleries.Add(newGallery);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Image gallery created successfully." : "Failed to create image gallery.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the image gallery: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<List<ImageGalleryModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<ImageGalleryModel>>();

            try
            {
                var data = await _context.GetData<ImageGalleryModel>("Exec [dbo].[SP_GetImageGallery]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ImageGalleryModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving image gallery: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<ImageGalleryModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<ImageGalleryModel>();

            try
            {
                var data = await _context.GetData<ImageGalleryModel>("exec [dbo].[SP_GetImageGalleryId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the image gallery: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(ImageGalleryDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingGallery = await _context.ImageGalleries.FindAsync(request.Id);

                if (existingGallery == null)
                {
                    response.Data = false;
                    response.Error = "Image gallery not found.";
                    return response;
                }

                existingGallery.Title = request.Title;
                existingGallery.Description = request.Description;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Image gallery updated successfully." : "Failed to update image gallery.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the image gallery: " + ex.Message;
            }

            return await Save(response);
        }

        private async Task<ServerResponse<bool>> Save(ServerResponse<bool> response)
        {
            try
            {
                int saveResult = await _context.SaveChangesAsync();
                if (saveResult > 0)
                {
                    response.Data = true;
                    response.SuccessMessage = "Image gallery updated successfully.";
                }
                else
                {
                    response.Data = false;
                    response.Error = "Failed to update image gallery.";
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while saving the image gallery: " + ex.Message;
            }

            return response;
        }
    }
}
