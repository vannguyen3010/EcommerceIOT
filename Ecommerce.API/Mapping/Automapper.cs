using AutoMapper;
using Entities.Models;
using Shared.DTO.CategoryIntroduce;
using Shared.DTO.Contact;
using Shared.DTO.Introduce;
using Shared.DTO.Profile;
using Shared.DTO.User;

namespace Ecommerce.API.Mapping
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<RegisterDto, User>();

            CreateMap<User, UserDto>();

            CreateMap<CreateContactDto, Contact>();

            CreateMap<Contact, ContactDto>();

            CreateMap<CreateProfileDto, ProfileInfo>();

            CreateMap<ProfileInfo, ProfileInfoDto>();

            CreateMap<UpdateProfile, ProfileInfo>();

            CreateMap<CreateCategoryIntroDto, CategoryNew>();

            CreateMap<CategoryNew, CategoryIntroduceDto>();

            CreateMap<UpdateCateIntroDto, CategoryNew>();

            CreateMap<CreateIntroduceDto, New>();

            CreateMap<New, IntroduceDto>();

            CreateMap<UpdateIntroduceDto, New>();


        }
    }
}
