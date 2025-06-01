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
    public class PlayerDamageLogger : IAnalyticsLogger, IEventListener<UnturnedPlayerDamagedEvent>
    {
        private readonly ILogger<PlayerDamageLogger> _logger;
        private readonly IWriterService _writer;

        public PlayerDamageLogger(ILogger<PlayerDamageLogger> logger, IWriterService writer)
        {
            _logger = logger;
            _writer = writer;
            _logger.LogInformation("Created player feed logger instance");
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

        public async Task HandleEventAsync(object? sender, UnturnedPlayerDamagedEvent @event) 
            => await _writer.AppendToDamageFile(@event);
    }
}