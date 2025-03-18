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
    public class LoginService:ILoginService
    {
        private readonly ILoginRepository _repository;
        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }

        //Login Authentication
        public async Task<(ResponseModel, ListUser)> Login(string username, string password)

        {
            return await _repository.Login(username, password);
        }
        // login permisssion version check
        public async Task<int?> GetPermissionVersionByUserIdAsync()
        {
            return await _repository.GetPermissionVersionByUserIdAsync();
        }
    }
}
