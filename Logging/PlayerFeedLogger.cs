using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;

namespace Scitalis.Analytics.Logging
{
    public class PlayerFeedLogger : IAnalyticsLogger
    {
        private readonly ILogger<PlayerFeedLogger> _logger;

        public PlayerFeedLogger(ILogger<PlayerFeedLogger> logger)
        {
            _logger = logger;
            _logger.LogInformation("Created player position logger instance");
        }

        public Task StartAsync(IOpenModPlugin plugin)
        {
            _logger.LogInformation("Starting player position logger");
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _logger.LogInformation("Stopping player position logger");
            return Task.CompletedTask;
        }
    }
}