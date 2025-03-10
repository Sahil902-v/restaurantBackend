using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aatithya_Core.Common;
//using Aatithya_Core.Models.RoleRights;
//using Aatithya_Core.Models.Screen;
using Aatithya_Core.Models.User;

namespace Aatithya_DAL.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<(ResponseModel, ListUserModel)> Login(string username, string password);
        //Task<int?> GetPermissionVersionByUserIdAsync();
    }
}
