using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;

namespace AatithyaB_BLL.Services.Interface
{
    public interface IUsersService
    {
        Task<ResponseModel> GetAllUser();
        Task<ResponseModel> GetUserById(int Id);
        Task<ResponseModel> InsertUser(AddUser model);
        Task<ResponseModel> UpdateUser(UpdateUser model);
        Task<ResponseModel> DeleteUser(int Id);
    }
}
