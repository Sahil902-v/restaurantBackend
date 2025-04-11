using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aatithya_Core.Models.Images
{
    public class ListImages
    {
        public int Id { get; set; }
        public string? ImgUrl { get; set; }
        public int ImgCategory { get; set; }
        public string? ImgTitle { get; set; }
        public string? ImgName { get; set; }
        public bool IsMainDisp { get; set; }
        public bool IsMenu { get; set; }
        public bool IsGallery { get; set; }
    }
}
