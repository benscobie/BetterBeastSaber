using AutoMapper;
using BetterBeastSaber.Data.Entities;

namespace BetterBeastSaber.Scraper.Mapping
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<Song, Domain.Song>();
            CreateMap<Domain.Song, Song>();

            CreateMap<Domain.UserRating, UserRating>();
            CreateMap<UserRating, Domain.UserRating>();
        }
    }
}
