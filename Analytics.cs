using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using Scitalis.Analytics.Logging;

[assembly: PluginMetadata("Scitalis.Analytics", DisplayName = "Player Analytics Logging")]
namespace Scitalis.Analytics
{
    public class Analytics : OpenModUnturnedPlugin
    {
        private readonly IEnumerable<IAnalyticsLogger> _analyticsLoggers;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly ILogger<Analytics> _logger;

        public Analytics(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<Analytics> logger,
            IServiceProvider serviceProvider, 
            IEnumerable<IAnalyticsLogger> analyticsLoggers) : base(serviceProvider)
        {
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
            _logger = logger;
            _analyticsLoggers = analyticsLoggers;
        }

        protected override async UniTask OnLoadAsync()
        {
            _logger.LogInformation($"Hello World!");
            _logger.LogInformation($"Found {_analyticsLoggers.Count()} loggers");
            foreach (var logger in _analyticsLoggers)
            {
                _logger.LogInformation($"Starting logger: {logger.GetType().Name}");
                await logger.StartAsync(this);
            }
        }

        protected override async UniTask OnUnloadAsync()
        {
            _logger.LogInformation(_stringLocalizer["plugin_events:plugin_stop"]);
            foreach (var logger in _analyticsLoggers)
            {
                await logger.StopAsync();
            }
        }
    }
}
