using Aatithya_Core.Common;
using Aatithya_Core.Models.Images;
using Azure;
using Microsoft.AspNetCore.Http;

namespace Aatithya_DAL.Repository.Interface
{
    public interface IImageRepository
    {
        Task<ResponseModel> GetAllImages();
        Task<ResponseModel> GetImageById(int id);

        Task<ResponseModel> InsertImage(AddImages img, IFormFile file);
        Task<ResponseModel> DeleteImage(int id);
    }
}
