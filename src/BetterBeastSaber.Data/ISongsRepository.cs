using BetterBeastSaber.Data.Entities;
using System.Threading.Tasks;

namespace BetterBeastSaber.Data
{
    public interface ISongsRepository
    {
        Task<bool> Update(Song song);
    }
}