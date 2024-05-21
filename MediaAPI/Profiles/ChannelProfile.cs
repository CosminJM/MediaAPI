using AutoMapper;
using Media.Domain;
using MediaAPI.Models;

namespace MediaAPI.Profiles
{
    public class ChannelProfile : Profile
    {
        public ChannelProfile()
        {
            CreateMap<ChannelForCreationDto, Channel>();
            CreateMap<Channel, ChannelForCreationDto>();
            CreateMap<Channel, ChannelDto>();
            CreateMap<ChannelDto, Channel>();
            CreateMap<Channel, ChannelDto>();
            CreateMap<ChannelForUpdateDto, Channel>();

        }
    }
}
