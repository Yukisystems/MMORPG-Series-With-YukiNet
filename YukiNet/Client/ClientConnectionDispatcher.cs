using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using YukiNet.PackageParser;

namespace YukiNet.Client
{
    public class ClientConnectionDispatcher
    {
        private readonly IPackageParser packageParser;
        private readonly NetworkConnection clientConnection;
        private Thread thread;

        private ConcurrentDictionary<string, Tuple<EventWaitHandle, _Holder>> waitingThreads =
            new ConcurrentDictionary<string, Tuple<EventWaitHandle, _Holder>>();

        public event EventHandler<IncomingPackageArgs> IncomingPackage;

        public ClientConnectionDispatcher(IPackageParser packageParser, NetworkConnection clientConnection)
        {
            this.packageParser = packageParser;
            this.clientConnection = clientConnection;

            IncomingPackage += ClientConnectionDispatcher_IncomingPackage;
        }

        private void ClientConnectionDispatcher_IncomingPackage(object sender, IncomingPackageArgs e)
        {
            // Debug.Log($"Incoming package: {e.Package.GetType().Name}");
        }

        public void Start()
        {
            thread = new Thread(HandlePackageInput);
            thread.Start();
        }

        private void HandlePackageInput(object obj)
        {
            while (true)
            {
                var package = packageParser.ParsePackageFromStream(clientConnection.Reader);
                IncomingPackage?.Invoke(this, new IncomingPackageArgs(package));
                foreach (var item in waitingThreads)
                {
                    if (item.Key.Equals(package.GetType().Name))
                    {
                        item.Value.Item2.Value = package;
                        item.Value.Item1.Set();
                    }
                }
            }
        }

        public Task<T> WaitForPackage<T>() where T : PackageBase
        {
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            waitingThreads.TryAdd(typeof(T).Name, new Tuple<EventWaitHandle, _Holder>(eventWaitHandle, new _Holder()));

            var task = Task.Run(() =>
            {
                eventWaitHandle.WaitOne(TimeSpan.FromSeconds(30));
                waitingThreads.TryRemove(typeof(T).Name, out var tuple);

                return (T)tuple.Item2.Value;
            });

            return task;
        }

        public void SendPackage(PackageBase package)
        {
            packageParser.ParsePackgeToStream(package, clientConnection.Writer);
        }

#pragma warning disable CA1034 // Nested types should not be visible

        public class _Holder
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public object Value { get; set; }
        }
    }

    public class IncomingPackageArgs : EventArgs
    {
        public IncomingPackageArgs()
        {
        }

        public IncomingPackageArgs(PackageBase package)
        {
            this.Package = package;
        }

        public PackageBase Package { get; private set; }
    }
}