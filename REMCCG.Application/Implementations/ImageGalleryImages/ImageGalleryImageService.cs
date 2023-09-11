using Azure;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using REMCCG.Application.Common.DTOs;
using REMCCG.Application.Common.Models;
using REMCCG.Application.Interfaces;
using REMCCG.Application.Interfaces.ImageGalleryImages;
using REMCCG.Domain.Entities;

namespace REMCCG.Application.Implementations.ImageGalleries
{
    public class ImageGalleryImageService : IImageGalleryImageService
    {
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;

        public ImageGalleryImageService(IAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<bool>> Create(ImageGalleryImageDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var newImage = request.Adapt<ImageGalleryImage>();

                _context.ImageGalleryImages.Add(newImage);
                int saveResult = await _context.SaveChangesAsync();

                response.Data = saveResult > 0;
                response.Error = response.Data ? "Image created successfully." : "Failed to create image.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while creating the image: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<List<ImageGalleryImageModel>>> GetAllRecord()
        {
            var response = new ServerResponse<List<ImageGalleryImageModel>>();

            try
            {
                var data = await _context.GetData<ImageGalleryImageModel>("Exec [dbo].[SP_GetImagefromgallery]");
                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ImageGalleryImageModel>();
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving image: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<ImageGalleryImageModel>> GetRecordById(object id)
        {
            var response = new ServerResponse<ImageGalleryImageModel>();

            try
            {
                var data = await _context.GetData<ImageGalleryImageModel>("exec [dbo].[SP_GetImagefromgalleryId]", new SqlParameter("@id", id));

                response.IsSuccessful = true;
                response.Data = data?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Error = "An error occurred while retrieving the Image: " + ex.Message;
            }

            return response;
        }

        public async Task<ServerResponse<bool>> Update(ImageGalleryImageDTO request)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingImage = await _context.ImageGalleryImages.FindAsync(request.Id);

                if (existingImage == null)
                {
                    response.Data = false;
                    response.Error = "Image not found.";
                    return response;
                }

                // Update the existing image with the new values
                existingImage.ImagePath = request.ImagePath;
                existingImage.GalleryID = request.GalleryID;

                int updateResult = await _context.SaveChangesAsync();

                response.Data = updateResult > 0;
                response.Error = response.Data ? "Image updated successfully." : "Failed to update image.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while updating the image: " + ex.Message;
            }

            return await Save(response);
        }

        public async Task<ServerResponse<bool>> Delete(object Id)
        {
            var response = new ServerResponse<bool>();

            try
            {
                var existingImage = await _context.ImageGalleryImages.FindAsync(Id);

                if (existingImage == null)
                {
                    response.Data = false;
                    response.Error = "Image not found.";
                    return response;
                }

                _context.ImageGalleryImages.Remove(existingImage);
                int deleteResult = await _context.SaveChangesAsync();

                response.Data = deleteResult > 0;
                response.Error = response.Data ? "Image deleted successfully." : "Failed to delete image.";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Error = "An error occurred while deleting the image: " + ex.Message;
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
                response.SuccessMessage = "Attendance record updated successfully.";
            }
            else
            {
                response.Data = false;
                response.Error = "Failed to update attendance record.";
            }
        }
        catch (Exception ex)
        {
            response.Data = false;
            response.Error = "An error occurred while saving the attendance record: " + ex.Message;
        }

        return response;
    }
}
}
