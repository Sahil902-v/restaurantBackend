using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AatithyaB_BLL.Services.Interface;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.Contact_Us;
using AatithyaB_Core.Models.User;
using AatithyaB_DAL.Repository.Interface;

namespace AatithyaB_BLL.Services.Implematiation
{
    public class Contact_UsService : IContact_UsService
    {
        private readonly IContact_UsRepository _repository;

        public Contact_UsService(IContact_UsRepository repository)
        {
            _repository = repository;
        }

        // Get All Contact_Us Details Method
        public async Task<ResponseModel> GetAllContact_Us()
        {
            return await _repository.GetAllContact_Us();
        }


        // Get Contact_Us Details By Id

        public async Task<ResponseModel> GetContact_UsById(int id)
        {
            return await _repository.GetContact_UsById( id);
        }


        //Insert Contact_Us Method

        public async Task<ResponseModel> InsertContact_Us(AddContacts_Us model)
        {
            return await _repository.InsertContact_Us( model);
        }


        // Delete User Method
        public async Task<ResponseModel> DeleteContact_Us(int Id)
        {
            return await _repository.DeleteContact_Us(Id);
        }

    }
}
