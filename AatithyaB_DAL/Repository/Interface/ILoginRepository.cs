using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;

namespace AatithyaB_DAL.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<(ResponseModel,ListUser)> Login(string username,string password);

        Task<int?> GetPermissionVersionByUserIdAsync();
    }
}
