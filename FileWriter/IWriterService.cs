using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Users;

namespace Scitalis.Analytics.FileWriter
{
    [Service]
    public interface IWriterService
    {
        Task AppendToPlayerPositionFile(ICollection<UnturnedUser> users);
        Task AppendToDamageFile(UnturnedPlayerDamagedEvent @event);
    }
}