using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Repository.Interface;

namespace Aatithya_BLL.Services.Implematiation
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
