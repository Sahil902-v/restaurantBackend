using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
//using Aatithya_Core.Models.Blogs;
//using Aatithya_Core.Models.Events;
//using Aatithya_Core.Models.Employees;
//using Aatithya_Core.Models.Organizations;
//using Aatithya_Core.Models.Services;
using Aatithya_Core.Models.User;
//using Aatithya_Core.Models.Department;
//using Aatithya_Core.Models.Role;
//using Aatithya_DAL.Entities;
//using Aatithya_Core.Models.AboutOrg;
//using Aatithya_Core.Models.Images;
//using Aatithya_Core.Models.CustomerDetail;
//using Aatithya_Core.Models.RoleRights;
//using Aatithya_DAL.Entities;
//using System.Data;
using System.Reflection.Metadata;
using Aatithya_DAL.Entities;

namespace Aatithya_DAL.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AddUserModel, User>();
            CreateMap<UpdateUserModel, User>();
            CreateMap<User, ListUserModel>();
        }
    }
}
