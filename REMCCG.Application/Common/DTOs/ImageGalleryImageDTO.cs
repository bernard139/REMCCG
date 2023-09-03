using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class ImageGalleryImageDTO : BaseObjectDTO
    {
        public int ID { get; set; }
        public string ImagePath { get; set; } // Path to the image in the gallery
        public int GalleryID { get; set; } // ID of the associated ImageGallery
        public ImageGallery Gallery { get; set; } // Navigation property for the associated ImageGallery
    }

    public class ImageGalleryImageModel
    {
        public int ID { get; set; }
        public string ImagePath { get; set; } // Path to the image in the gallery
        public int GalleryID { get; set; } // ID of the associated ImageGallery
        public ImageGallery Gallery { get; set; } // Navigation property for the associated ImageGallery
    }
}
