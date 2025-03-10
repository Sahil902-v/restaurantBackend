using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_DAL.Repository.Interface;
using Aatithya_BLL.Services.Interface;
using Aatithya_Core.Common;
//using Aatithya_Core.Models.RoleRights;
//using Aatithya_Core.Models.Screen;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Repository.Interface;
using Aatithya_Core.Common;

namespace Aatithya_BLL.Services.Implematiation
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repository;
        public LoginService(ILoginRepository repository)
        {
            _repository = repository;
        }

        #region Login Authentication
        /// <summary>
        ///  Retrieves the user login from the information database based on the provided username and password.
        /// </summary>
        /// <remarks>
        /// This method fetches the user login information from the database for authentication purposes.
        /// </remarks>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <param name="password">The password of the user attempting to log in.</param>
        public async Task<(ResponseModel, ListUserModel)> Login(string username, string password)
        {
            return await _repository.Login(username, password);
        }
        #endregion
    }
}
