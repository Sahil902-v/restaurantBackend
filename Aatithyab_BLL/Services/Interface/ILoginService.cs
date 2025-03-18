using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AatithyaB_Core.Common;
using AatithyaB_Core.Models.User;

namespace AatithyaB_BLL.Services.Interface
{
    public interface ILoginService
    {
        Task<(ResponseModel, ListUser)> Login(string username, string password);

        Task<int?> GetPermissionVersionByUserIdAsync();
    }
}
