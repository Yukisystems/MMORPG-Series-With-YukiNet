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
using System.IO;
using System.Net.Sockets;

namespace YukiNet.Client
{
    public class NetworkConnection
    {
        public Guid ConnectionId { get; private set; }

        private TcpClient tcpClient;
        private NetworkStream stream;

        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }

        public bool NeedsToConnect => tcpClient == null;

        public NetworkConnection(Guid ConnectionId, TcpClient tcpClient)
        {
            this.ConnectionId = ConnectionId;
            this.tcpClient = tcpClient;

            this.stream = tcpClient.GetStream();
            this.Reader = new BinaryReader(stream);
            this.Writer = new BinaryWriter(stream);
        }

        /// <summary>
        /// Creates a uninitalized networkconnection needs to call connect!
        /// </summary>
        public NetworkConnection()
        {
        }

        public int AvaibleBytes => tcpClient.Available;
        public bool IsConnected => tcpClient.Connected;

        public TcpClient Client => tcpClient;

        public void Connect(string address, int port)
        {
            if (!NeedsToConnect)
            {
                throw new InvalidOperationException("Already Connected to a host");
            }
            tcpClient = new TcpClient(address, port);

            this.stream = tcpClient.GetStream();
            this.Reader = new BinaryReader(stream);
            this.Writer = new BinaryWriter(stream);
        }
    }
}