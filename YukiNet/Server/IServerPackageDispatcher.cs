using YukiNet.Client;
using YukiNet.PackageParser;

namespace YukiNet.Server
{
    public interface IServerPackageDispatcher
    {
        bool Running { get; }

        void DispatchPackage(PackageBase package, NetworkConnection connection);

        void Start();
    }
}