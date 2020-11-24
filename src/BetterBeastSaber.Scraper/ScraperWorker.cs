using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetterBeastSaber.Scraper
{
    public class ScraperWorker : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<ScraperWorker> _logger;
        private readonly Scraper _scraper;
        private Timer _timer;

        public ScraperWorker(ILogger<ScraperWorker> logger, Scraper scraper)
        {
            _logger = logger;
            _scraper = scraper;
        }
        
        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scraper Hosted Service running.");

            await DoWork();
        }
        
        private async Task DoWork()
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);

            await _scraper.ExecuteAsync();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}