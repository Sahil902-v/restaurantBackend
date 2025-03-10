using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;
using Aatithya_Core.Common;

namespace Aatithya_BLL.Services.Interface
{
    public interface IUsersService
    {
        //Task<List<UsersModel>> GetAllUsers();
        Task<ResponseModel> GetAllUsers();
        Task<ResponseModel> GetUserById(int Id);
        Task<ResponseModel> InsertUser(AddUserModel model);
        Task<ResponseModel> UpdateUser(UpdateUserModel model);
        Task<ResponseModel> DeleteUser(int Id);
    }
}
