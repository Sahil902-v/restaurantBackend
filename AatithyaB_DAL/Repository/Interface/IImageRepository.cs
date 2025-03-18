using AatithyaB_Core.Common;
using AatithyaB_Core.Models.Images;
using Azure;

namespace AatithyaB_DAL.Repository.Interface
{
    public interface IImageRepository
    {
        Task<ResponseModel> GetAllImages();
        Task<ResponseModel> GetImageById(int id);

        Task<ResponseModel> InsertImage(AddImages img, IFormFile file);
        Task<ResponseModel> DeleteImage(int id);
    }
}
