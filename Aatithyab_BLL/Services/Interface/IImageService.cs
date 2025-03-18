using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.Images;
using Microsoft.AspNetCore.Http;

namespace AatithyaB_BLL.Services.Interface
{
    public interface IImageService
    {
        Task<ResponseModel> GetAllImages();
        Task<ResponseModel> GetImageById(int id);

        Task<ResponseModel> InsertImage(AddImages img, IFormFile file);
        Task<ResponseModel> DeleteImage(int id);
    }
}
