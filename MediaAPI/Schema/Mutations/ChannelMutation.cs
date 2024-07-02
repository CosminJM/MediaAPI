using AutoMapper;
using HotChocolate.Authorization;
using Media.DataAccess.Repository;
using Media.Domain;
using MediaAPI.Middlewares;
using MediaAPI.Models;

namespace MediaAPI.Schema.Mutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class ChannelMutation
    {
        private readonly IChannelsRepository _channelsRepository;
        private readonly IMapper _mapper;

        public ChannelMutation(IChannelsRepository channelsRepository, IMapper mapper)
        {
            _channelsRepository = channelsRepository;
            _mapper = mapper;
        }

        [Authorize]
        [UseUser]
        public async Task<GqlResult<ChannelDto>> AddChannelAsync([User] User user, ChannelForCreationDto channelForCreationDto) // ChannelForCreationDto should be ChannelForCreationDtoInput on mutation string when used as paramenter on client side
        {
            var channelsExists = await _channelsRepository.ChannelForUserExistsAsync(channelForCreationDto.ChannelIdentificator, user.Username);
            if (channelsExists)
            {
                return GqlResult<ChannelDto>.Failure("Channel exists.");
            }
            var channel = _mapper.Map<Channel>(channelForCreationDto);
            await _channelsRepository.AddChannelWithUserAsync(channel, user.Username);
            await _channelsRepository.SaveChangesAsync();

            return GqlResult<ChannelDto>.Success(_mapper.Map<ChannelDto>(channel));
        }

        [Authorize]
        [UseUser]
        public async Task<GqlResult<ChannelDto>> UpdateChannelAsync([User] User user, ChannelForUpdateDto channelForUpdateDto)
        {
            var existingChannel = await _channelsRepository.GetByIdAndUserAsync(channelForUpdateDto.Id, user.Username);
            if (existingChannel == null)
            {
                return GqlResult<ChannelDto>.Failure("Channel does not exist.");
            }
            _mapper.Map(channelForUpdateDto, existingChannel);
            await _channelsRepository.SaveChangesAsync();
            return GqlResult<ChannelDto>.Success(_mapper.Map<ChannelDto>(existingChannel));
        }

        [Authorize]
        [UseUser]
        public async Task<GqlResult<ChannelDto>> DeleteChannelAsync([User] User user, Guid channelId)
        {
            var existingChannel = await _channelsRepository.GetByIdAndUserAsync(channelId, user.Username);
            if (existingChannel == null)
            {
                return GqlResult<ChannelDto>.Failure("Channel does not exist.");
            }
            _channelsRepository.Delete(existingChannel);
            await _channelsRepository.SaveChangesAsync();
            return GqlResult<ChannelDto>.Success(null);
        }

    }
}
