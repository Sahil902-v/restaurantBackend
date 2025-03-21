using Aatithya_Core.Models.Contact_Us;
using Aatithya_Core.Models.Images;
using Aatithya_Core.Models.User;
using Aatithya_DAL.Entities;
using AutoMapper;

namespace Aatithya_DAL.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<AddUser, User>();
            CreateMap<User, ListUser>();
            CreateMap<UpdateUser, User>();
            
            CreateMap<AddImages, Image>();
            CreateMap<Image, ListImages>();

            CreateMap<AddContacts_Us, ContactU>();
            CreateMap<ContactU, ListContacts_Us>();
            CreateMap<UpdateContacts_Us, ContactU>();
        }
    }
}
