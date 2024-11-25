using AutoMapper;
using Entities.Models;
using Shared.DTO.Contact;
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

        }
    }
}
