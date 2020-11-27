using AngleSharp;
using AutoMapper;
using BetterBeastSaber.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using BetterBeastSaber.Mapping;

namespace BetterBeastSaber.Scraper
{
    class Program
    {
        static Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
              .ConfigureServices((context, services) =>
              {
                  services.AddOptions();
                  services.Configure<BetterBeastSaberDatabaseSettings>(context.Configuration.GetSection("MongoConnection"));
                  services.AddSingleton(BrowsingContext.New(AngleSharp.Configuration.Default.WithDefaultLoader()));
                  services.AddSingleton(typeof(Scraper));
                  services.AddSingleton<IBetterBeastSaberContext, BetterBeastSaberContext>();
                  services.AddTransient<ISongsRepository, SongsRepository>();
                  services.AddAutoMapper(typeof(EntityProfile));
                  services.AddHostedService<ScraperWorker>();
              })
              .Build();

            return host.RunAsync();
        }
    }
}
