using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_Core.Common;
//using Aatithya_Core.Models.RoleRights;
//using Aatithya_Core.Models.Screen;
using Aatithya_Core.Models.User;
using Aatithya_Core.Common;

namespace Aatithya_BLL.Services.Interface
{
    public interface ILoginService
    {
        Task<(ResponseModel, ListUserModel)> Login(string username, string password);
        //Task<int?> GetPermissionVersionByUserIdAsync();
    }
}
