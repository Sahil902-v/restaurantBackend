using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_Core.Common;
using Aatithya_Core.Models.User;

namespace Aatithya_BLL.Services.Interface
{
    public interface ILoginService
    {
        Task<(ResponseModel, ListUser)> Login(string username, string password);

        Task<int?> GetPermissionVersionByUserIdAsync();
    }
}
