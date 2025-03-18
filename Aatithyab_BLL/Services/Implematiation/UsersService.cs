using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AatithyaB_BLL.Services.Interface;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;
using AatithyaB_DAL.Repository.Interface;

namespace AatithyaB_BLL.Services.Implematiation
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;

        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }

        // Get All Users Method
        public async Task<ResponseModel> GetAllUser()
        {
            return await _repository.GetAllUser();
        }


        // Get User By Id

        public async Task<ResponseModel> GetUserById(int Id)
        {
            return await _repository.GetUserById(Id);
        }


        //Insert User Method

        public async Task<ResponseModel> InsertUser(AddUser model)
        {
            return await _repository.InsertUser(model);
        }

        //Update User Method

        public async Task<ResponseModel> UpdateUser(UpdateUser model)
        {
            return await _repository.UpdateUser(model);
        }

        // Delete User Method
        public async Task<ResponseModel> DeleteUser(int Id)
        {
            return await _repository.DeleteUser(Id);
        }

    }
}