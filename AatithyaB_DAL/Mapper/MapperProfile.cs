using AatithyaB_Core.Models.Contact_Us;
using AatithyaB_Core.Models.Images;
using AatithyaB_Core.Models.User;
using AatithyaB_DAL.Entities;
using AutoMapper;

namespace AatithyaB_DAL.Mapper
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
