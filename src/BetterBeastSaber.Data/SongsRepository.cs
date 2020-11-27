using BetterBeastSaber.Data.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace BetterBeastSaber.Data
{
    public class SongsRepository : ISongsRepository
    {
        private readonly IBetterBeastSaberContext _context;

        public SongsRepository(IBetterBeastSaberContext betterBeastSaberContext)
        {
            _context = betterBeastSaberContext;
        }

        public async Task<bool> Update(Song song)
        {
            var actionResult = await _context.Songs
                .ReplaceOneAsync(n => n.Id.Equals(song.Id), song, new ReplaceOptions { IsUpsert = true });

            return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
        }
    }
}