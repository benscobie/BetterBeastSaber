using System.Collections.Generic;
using BetterBeastSaber.Data.Entities;
using System.Threading.Tasks;

namespace BetterBeastSaber.Data
{
    public interface ISongsRepository
    {
        Task<bool> UpdateAsync(Song song);

        Task<List<Song>> GetAsync(int offset, int limit);
    }
}