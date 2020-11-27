using AutoMapper;
using BetterBeastSaber.Data;
using BetterBeastSaber.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetterBeastSaber.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongsController : ControllerBase
    {
        
        private readonly ISongsRepository _songsRepository;
        private readonly IMapper _mapper;

        public SongsController(ISongsRepository songsRepository, IMapper mapper)
        {
            _songsRepository = songsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Song>> Get(int offset, int limit)
        {
            var songEntities = await _songsRepository.GetAsync(offset, limit);

            return _mapper.Map<List<Song>>(songEntities);
        }
    }
}