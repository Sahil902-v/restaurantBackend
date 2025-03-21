using Aatithya_Core.Common;
using Aatithya_Core.Models.User;

namespace Aatithya_DAL.Repository.Interface
{
    public interface IUsersRepository
    {
        Task<ResponseModel> GetAllUser();
        Task<ResponseModel> GetUserById(int Id);
        Task<ResponseModel> InsertUser(AddUser model);
        Task<ResponseModel> UpdateUser(UpdateUser model);
        Task<ResponseModel> DeleteUser(int Id);

    }
}
