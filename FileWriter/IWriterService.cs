using System.Collections.Generic;
using System.Threading.Tasks;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Users;

namespace Scitalis.Analytics.FileWriter
{
    [Service]
    public interface IWriterService
    {
        Task AppendPlayerPositionToFile(ICollection<UnturnedUser> users);
    }
}