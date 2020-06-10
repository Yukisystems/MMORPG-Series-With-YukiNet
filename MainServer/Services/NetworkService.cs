///////////////////////////////////////////////////////////////////////////////////////
//  _______ _            _______    _             _       _    _____ _           __  //
// |__   __| |          |__   __|  | |           (_)     | |  / ____| |         / _| //
//    | |  | |__   ___     | |_   _| |_ ___  _ __ _  __ _| | | |    | |__   ___| |_  //
//    | |  | '_ \ / _ \    | | | | | __/ _ \| '__| |/ _` | | | |    | '_ \ / _ \  _| //
//    | |  | | | |  __/    | | |_| | || (_) | |  | | (_| | | | |____| | | |  __/ |   //
//    |_|  |_| |_|\___|    |_|\__,_|\__\___/|_|  |_|\__,_|_|  \_____|_| |_|\___|_|   //
//                                                                                   //
//     The Tutorial Chef : https://youtube.com/c/TheTutorialChef                     //
//     Website:            https://thetutorialchef.com                               //
//     Twitter:            https://twitter.com/thetutorialchef                       //
//     Patreon:            https://www.patreon.com/thetutorialchef                   //
//     Discord:            https://discord.gg/kGrRQJ9                                //
//                                                                                   //
//     Company:            https://yukisystems.com                                   //
//                                                                                   //
//         Copyright by Deadlyviruz aka The Tutorialchef                             //
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YukiNet.Client;
using YukiNet.PackageParser;
using YukiNet.PackageParser.PackageImplementaions;
using YukiNet.Server;

namespace MainServer.Serivces
{
    internal sealed class NetworkService
    {
        private readonly object AddRemoveLocker = new object();
        private readonly IServerPackageDispatcher packageDispatcher;
        private readonly IPackageParser packageParser;
        private readonly IServiceProvider serviceProvider;

        private readonly List<NetworkConnection> clientConnections = new List<NetworkConnection>();
        private Thread clientPackageReceiverThread;
        private readonly List<NetworkConnection> invalidConnections = new List<NetworkConnection>();
        private Thread listenerWorkerThread;

        public ILogger logger;

        private int receivePackageInterationCounter;

        private readonly TcpListener tcpListener;

        public NetworkService(IConfigurationRoot config, ILogger<NetworkService> logger
            , IServiceProvider serviceProvider, IPackageParser packageParser,
            IServerPackageDispatcher packageDispatcher)
        {
            tcpListener = new TcpListener(
                IPAddress.Parse(config.GetValue<string>("host")),
                config.GetValue<int>("port"));
            Running = false;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.packageParser = packageParser;
            this.packageDispatcher = packageDispatcher;
        }

        public NetworkService(IPAddress address, int port,
            ILogger<NetworkService> logger, IServiceProvider serviceProvider)
        {
            tcpListener = new TcpListener(address, port);
            Running = false;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public bool Running { get; private set; }

        public event Action<TcpClient> ClientConnected;

        public void Start()
        {
            if (listenerWorkerThread != null && listenerWorkerThread.ThreadState == ThreadState.Running)
                return; // bereits am laufen

            listenerWorkerThread = new Thread(async () =>
            {
                Running = true;
                tcpListener.Start();

                logger.LogInformation("NetworkSerivce Sucessfully started");
                //      Console.WriteLine("Server Started...");
                try
                {
                    while (Running)
                    {
                        await Task.Delay(100);
                        if (Running && tcpListener.Pending())
                            OnClientConnected(await tcpListener.AcceptTcpClientAsync());
                    }
                }
                finally
                {
                    logger.LogError("Server Stopped...");
                    //   System.Console.WriteLine("SERVER STOPPED");
                }
            })
            {
                IsBackground = true
            };
            listenerWorkerThread.Start();

            clientPackageReceiverThread = new Thread(ReceivePackage)
            {
                IsBackground = true
            };
            clientPackageReceiverThread.Start();
        }

        private async void ReceivePackage()
        {
            try
            {
                logger.LogInformation("NetworkSerivce.receivePackage thread Sucessfully started");
                while (Running)
                {
                    await Task.Delay(1);
                    if (Running)
                    {
                        lock (AddRemoveLocker)
                        {
                            if (++receivePackageInterationCounter == 1000)
                            {
                                receivePackageInterationCounter = 0;
                                foreach (var client in clientConnections)
                                    try
                                    {
                                        packageParser.ParsePackgeToStream(new KeepAlivePackage(), client.Writer);
                                    }
                                    catch (Exception)
                                    {
                                        invalidConnections.Add(client);
                                        logger.LogInformation("KEEPALIVE EXCEPTION");
                                    }
                            }

                            if (invalidConnections.Count > 0)
                            {
                                foreach (var conn in invalidConnections) clientConnections.Remove(conn);

                                invalidConnections.Clear();
                            }
                        }

                        var clientConArr = clientConnections.ToArray();

#pragma warning disable U2U1203 // Use foreach efficiently
                        foreach (var client in clientConArr)
#pragma warning restore U2U1203 // Use foreach efficiently
                            if (client.AvaibleBytes > 0)
                            {
                                logger.LogInformation($"Packge from Client {client.ConnectionId}");

                                var package = packageParser.ParsePackageFromStream(client.Reader);
                                packageDispatcher.DispatchPackage(package, client);
                            }
                    }
                }
            }
            finally
            {
                logger.LogError("Server Stopped...");
                //   System.Console.WriteLine("SERVER STOPPED");
            }
        }

        public void Stop()
        {
            if (listenerWorkerThread == null) return; // bereits gestoppt

            Running = false;
            listenerWorkerThread.Abort();
            listenerWorkerThread = null;
            clientPackageReceiverThread.Abort();
            clientPackageReceiverThread = null;
            tcpListener.Stop();
        }

        private void OnClientConnected(TcpClient connection)
        {
            ClientConnected?.Invoke(connection);
            var client = new NetworkConnection(Guid.NewGuid(), connection);
            lock (AddRemoveLocker)
            {
                clientConnections.Add(client);
            }

            logger.LogInformation($"New Client connected Guid: {client.ConnectionId}");
        }
    }
}