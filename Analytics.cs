using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;

[assembly: PluginMetadata("Scitalis.Analytics", DisplayName = "Player Analytics Logging")]
namespace Scitalis.Analytics
{
    public class Analytics : OpenModUnturnedPlugin
    {
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly ILogger<Analytics> _logger;

        public Analytics(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<Analytics> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _configuration = configuration;
            _stringLocalizer = stringLocalizer;
            _logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            _logger.LogInformation("Hello World!");
        }

        protected override async UniTask OnUnloadAsync()
        {
            _logger.LogInformation(_stringLocalizer["plugin_events:plugin_stop"]);
        }
    }
}
