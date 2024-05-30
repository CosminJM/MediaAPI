using AutoMapper;
using Media.DataAccess.Repository;
using Media.Domain;
using MediaAPI.Extensions;
using MediaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediaAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    [EnableCors("AllowPolicy")]
    [Authorize]
    public class ChannelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChannelsRepository _channelsRepository;
        private readonly IUserRepository _userRepository;

        public ChannelsController(IMapper mapper, IChannelsRepository channelsRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _channelsRepository = channelsRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetChannels()
        {
            var username = this.GetUsernameFromToken();
            if (username == null)
            {
                return NotFound();
            }
            var channels = await _channelsRepository.GetAllByUserAsync(username);

            var channelDto = _mapper.Map<List<ChannelDto>>(channels);

            return Ok(channelDto);
        }

        [HttpGet("{id}", Name = "GetChannel")]
        public async Task<IActionResult> GetChannel(int id)
        {
            var channel = await _channelsRepository.GetByIdAsync(id);
            if (channel == null)
            {
                return NotFound();
            }
            var channelWithoutVideosDto = _mapper.Map<ChannelDto>(channel);
            return Ok(channelWithoutVideosDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddChannel(ChannelForCreationDto channelForCreationDto)
        {
            var username = this.GetUsernameFromToken();
            if (username == null)
            {
                return NotFound();
            }
            if (await _channelsRepository.ChannelForUserExistsAsync(channelForCreationDto.ChannelIdentificator, username))
            {
                return BadRequest("Channel already exists.");
            }

            var channel = _mapper.Map<Channel>(channelForCreationDto);

            await _channelsRepository.AddChannelWithUserAsync(channel, username);
            await _channelsRepository.SaveChangesAsync();

            //Maybe return this with CreatedAtRoute()
            var channelToReturn = _mapper.Map<ChannelDto>(channel);

            return CreatedAtRoute("GetChannel", new { id = channelToReturn.ChannelId }, channelToReturn);
        }

        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdateChannel(int channelId, ChannelForUpdateDto channelForUpdateDto)
        {
            var username = this.GetUsernameFromToken();
            if (username == null)
            {
                return NotFound();
            }
            var existingChannel = await _channelsRepository.GetByIdAndUserAsync(channelId, username);
            if (existingChannel == null)
            {
                return NotFound();
            }

            _mapper.Map(channelForUpdateDto, existingChannel);

            await _channelsRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{channelId}")]
        public async Task<IActionResult> DeleteChannel(int channelId)
        {
            var username = this.GetUsernameFromToken();
            if (username == null)
            {
                return NotFound();
            }
            var existingChannel = await _channelsRepository.GetByIdAndUserAsync(channelId, username);
            if (existingChannel == null)
            {
                return NotFound();
            }

            _channelsRepository.Delete(existingChannel);
            await _channelsRepository.SaveChangesAsync();
            return NoContent();
        }

    }
}
