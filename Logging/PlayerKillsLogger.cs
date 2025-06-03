using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Eventing;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Players.Life.Events;
using Scitalis.Analytics.FileWriter;

namespace Scitalis.Analytics.Logging
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
    public class PlayerKillsLogger : IAnalyticsLogger, IEventListener<UnturnedPlayerDeathEvent>
    {
        private readonly IWriterService _writer;
        private readonly ILogger<PlayerKillsLogger> _logger;

        public PlayerKillsLogger(IWriterService writer, ILogger<PlayerKillsLogger> logger)
        {
            _writer = writer;
            _logger = logger;
        }

        public Task StartAsync(IOpenModPlugin plugin)
        {
            _logger.LogInformation("Starting player feed logger");
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _logger.LogInformation("Stopping player feed logger");
            return Task.CompletedTask;
        }

        public Task HandleEventAsync(object? sender, UnturnedPlayerDeathEvent @event)
            => _writer.AppendToKillFeedFile(@event);
    }
}