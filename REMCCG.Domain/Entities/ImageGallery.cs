using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Domain.Entities
{
    public class ImageGallery
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ImageGalleryImage> Images { get; set; }
    }

}
