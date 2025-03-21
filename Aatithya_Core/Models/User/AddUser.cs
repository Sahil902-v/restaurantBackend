using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aatithya_Core.Models.User
{
    public class AddUser
    {
       
        public string Username { get; set; }
        public string Password { get; set; }
        public int PermissionVersion { get; set; }

    }
}
