using Aatithya_Core.Common;
using Aatithya_Core.Models.User;

namespace Aatithya_DAL.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<(ResponseModel,ListUser)> Login(string username,string password);

        Task<int?> GetPermissionVersionByUserIdAsync();
    }
}
