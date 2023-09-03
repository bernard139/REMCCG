using REMCCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Common.DTOs
{
    public class ImageGalleryDTO : BaseObjectDTO
    {
            public int ID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public List<ImageGalleryImage> Images { get; set; }
    }

    public class ImageGalleryModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ImageGalleryImage> Images { get; set; }
    }
}
