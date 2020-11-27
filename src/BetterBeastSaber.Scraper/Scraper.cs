using AngleSharp;
using System.Threading.Tasks;
using AutoMapper;
using BetterBeastSaber.Data;
using BetterBeastSaber.Data.Entities;
using Microsoft.Extensions.Logging;

namespace BetterBeastSaber.Scraper
{
    public class Scraper
    {
        private readonly ILogger<Scraper> _logger;
        private readonly IBrowsingContext _browsingContext;
        private readonly ISongsRepository _songsRepository;
        private readonly IMapper _mapper;

        public Scraper(ILogger<Scraper> logger, IBrowsingContext browsingContext, ISongsRepository songsRepository, IMapper mapper)
        {
            _logger = logger;
            _browsingContext = browsingContext;
            _songsRepository = songsRepository;
            _mapper = mapper;
        }

        public async Task ExecuteAsync()
        {
            var baseUrl = "https://bsaber.com/songs/";
            var page = 1;
            bool parsedSongs;

            do
            {
                var navigationUrl = baseUrl + $"page/{page}/";
                var document = await _browsingContext.OpenAsync(navigationUrl);

                var extractor = new Extractor(document);
                var songs = extractor.ExtractSongs();

                foreach (var song in songs)
                {
                    var songEntity = _mapper.Map<Song>(song);
                    await _songsRepository.UpdateAsync(songEntity);
                }

                page++;
                parsedSongs = songs.Count > 0; // TODO Properly detect last page
            } while (parsedSongs);
        }
    }
}