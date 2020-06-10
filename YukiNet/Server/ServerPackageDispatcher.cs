using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using YukiNet.Client;
using YukiNet.PackageParser;

namespace YukiNet.Server
{
    public class ServerPackageDispatcher : IServerPackageDispatcher
    {
        private readonly ILogger<ServerPackageDispatcher> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IPackageParser packageParser;

        private BlockingCollection<Tuple<NetworkConnection, PackageBase>> queue
            = new BlockingCollection<Tuple<NetworkConnection, PackageBase>>();

        public bool Running { get; private set; }

        private Thread dispatcherThread;

        public ServerPackageDispatcher(ILogger<ServerPackageDispatcher> logger, IServiceProvider serviceProvider,
            IPackageParser packageParser)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.packageParser = packageParser;
        }

        public void Start()
        {
            dispatcherThread = new Thread(() =>
            {
                logger.LogInformation("Dispatcher Started...");
                Running = true;
                try
                {
                    while (Running)
                    {
                        if (queue.TryTake(out var item))
                        {
                            using (var scope = serviceProvider.CreateScope())
                            {
                                var (connection, package) = item;
                                var connHandler = scope.ServiceProvider.GetRequiredService<ConnectionHandlerBase<NetworkConnection>>();
                                connHandler.InvokeAction(connection, package, package.ID);
                            }
                        }
                    }
                }
                finally
                {
                    logger.LogError("Dispatcher Stopped...");
                    //   System.Console.WriteLine("SERVER STOPPED");
                }
            })
            { IsBackground = true, };
            dispatcherThread.Start();
        }

        public void DispatchPackage(PackageBase package, NetworkConnection connection)
        {
            queue.Add(new Tuple<NetworkConnection, PackageBase>(connection, package));
        }
    }

    public static class ServerPackageDispatcherExtensions
    {
        public static void AddServerPackageDispatcher(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddSingleton<IServerPackageDispatcher>((sp) =>
            {
                var obj = ActivatorUtilities.CreateInstance<ServerPackageDispatcher>(sp);
                obj.Start();
                return obj;
            });
        }
    }
}