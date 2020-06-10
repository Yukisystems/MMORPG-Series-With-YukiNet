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
using MainServer.Services.WebsocketServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using WebSocketSharp.Server;

namespace MainServer
{
    public class ConfigurationService
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public static ConfigurationService CreateInstance()
        {
            return CreateInstance(s => { });
        }

        public static ConfigurationService CreateInstance(Action<IServiceCollection> handler)
        {
            var instance = new ConfigurationService();

            var descriptors = CreateDefaultSericeDescriptors();
            handler(descriptors);

            instance.ServiceProvider = descriptors.BuildServiceProvider();
            return instance;
        }

        // Server Config file load
        private static IServiceCollection CreateDefaultSericeDescriptors()
        {
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddJsonFile("worldserver/json/worldserver.json")
                .Build();

            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddLogging(b => b.AddNLog());
            serviceDescriptors.AddSingleton(Configuration);

            serviceDescriptors.RegisterWebSocketBehavior<EchoService>();

            return serviceDescriptors;
        }
    }
}