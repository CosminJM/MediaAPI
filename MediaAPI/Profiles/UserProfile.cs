using AutoMapper;
using Media.Domain;
using MediaAPI.Models;

namespace MediaAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
