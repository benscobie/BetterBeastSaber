using BetterBeastSaber.Data.Entities;
using MongoDB.Driver;

namespace BetterBeastSaber.Data
{
    public interface IBetterBeastSaberContext
    {
        IMongoCollection<Song> Songs { get; }
    }
}