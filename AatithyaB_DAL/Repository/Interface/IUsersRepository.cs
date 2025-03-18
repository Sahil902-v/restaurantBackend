using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;

namespace AatithyaB_DAL.Repository.Interface
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
