using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.Images;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Repository.Interface;
using Microsoft.AspNetCore.Http;

namespace Aatithya_BLL.Services.Implematiation
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _repository;

        public ImageService(IImageRepository repository)
        {
            _repository = repository;
        }

        // Get All Image Method
        public async Task<ResponseModel> GetAllImages()
        {
            return await _repository.GetAllImages(); ;
        }


        // Get Image By Id

        public async Task<ResponseModel> GetImageById(int id)
        {
            return await _repository.GetImageById(id); 
        }


        //Insert Image Method

        public async Task<ResponseModel> InsertImage(AddImages img, IFormFile file)
        {
            return await _repository.InsertImage(img, file);
        }

        // Delete Image Method
        public async Task<ResponseModel> DeleteImage(int id)
        {
            return await _repository.DeleteImage(id);
        }

    }
}
