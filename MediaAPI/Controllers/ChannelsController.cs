using AutoMapper;
using Media.DataAccess.Repository;
using Media.Domain;
using MediaAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    [EnableCors("AllowPolicy")]
    public class ChannelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChannelsRepository _channelsRepository;

        public ChannelsController(IMapper mapper, IChannelsRepository channelsRepository)
        {
            _mapper = mapper;
            _channelsRepository = channelsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetChannels()
        {
            var channels = await _channelsRepository.GetAllAsync();
            return Ok(channels);
        }

        [HttpGet("{id}", Name = "GetChannel")]
        public async Task<IActionResult> GetChannel(int id)
        {
            var channel = await _channelsRepository.GetByIdAsync(id);
            if(channel == null)
            {
                return NotFound();
            }
            var channelWithoutVideosDto = _mapper.Map<ChannelDto>(channel);
            return Ok(channelWithoutVideosDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddChannel(ChannelForCreationDto channelForCreationDto)
        {
            if(await _channelsRepository.ChannelExistsAsync(channelForCreationDto.ChannelIdentificator))
            {
                return Ok("Channel exists.");
            }

            var channel = _mapper.Map<Channel>(channelForCreationDto);

            await _channelsRepository.AddAsync(channel);
            await _channelsRepository.SaveChangesAsync();

            //Maybe return this with CreatedAtRoute()
            var channelToReturn = _mapper.Map<ChannelDto>(channel);

            return CreatedAtRoute("GetChannel",new {id = channelToReturn.ChannelId }, channelToReturn);
        }

        [HttpPut("{channelId}")]
        public async Task<IActionResult> UpdateChannel(int channelId, ChannelForUpdateDto channelForUpdateDto)
        {
            var existingChannel = await _channelsRepository.GetByIdAsync(channelId);
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
            var existingChannel = await _channelsRepository.GetByIdAsync(channelId);
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
