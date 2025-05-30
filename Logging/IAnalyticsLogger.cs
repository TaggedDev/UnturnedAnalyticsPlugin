using System.Threading.Tasks;
using OpenMod.API.Ioc;
using OpenMod.API.Plugins;

namespace Scitalis.Analytics.Logging
{
    [Service]
    public interface IAnalyticsLogger
    {
        Task StartAsync(IOpenModPlugin plugin);
        Task StopAsync();
    }
}