using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;
using OpenMod.Core.Helpers;
using OpenMod.Unturned.Users;
using Scitalis.Analytics.FileWriter;

namespace Scitalis.Analytics.Logging
{
    [ServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
    public class PlayerPositionLogger : IAnalyticsLogger
    {
        private readonly ILogger<PlayerPositionLogger> _logger;
        private readonly IUnturnedUserDirectory _unturnedUsers;
        private readonly IWriterService _writer;

        public PlayerPositionLogger(ILogger<PlayerPositionLogger> logger,
            IUnturnedUserDirectory unturnedUsers,
            IWriterService writer)
        {
            _logger = logger;
            _unturnedUsers = unturnedUsers;
            _writer = writer;
        }

        public Task StartAsync(IOpenModPlugin plugin)
        {
            _logger.LogInformation("Started player position logger");
            AsyncHelper.Schedule("PlayerPositionChecker", () => RunPositionParseLoop(plugin));
            return Task.CompletedTask;
        }
        

        public Task StopAsync()
        {
            _logger.LogInformation("Stopping player position logger");
            return Task.CompletedTask;
        }

        private async Task RunPositionParseLoop(IOpenModPlugin plugin)
        {
            while (plugin.IsComponentAlive)
            {
                ICollection<UnturnedUser> users = _unturnedUsers.GetOnlineUsers();
                if (users.Count != 0)
                    await _writer.AppendToPlayerPositionFile(users);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}