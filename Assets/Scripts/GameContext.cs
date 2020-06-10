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
using Microsoft.Extensions.DependencyInjection;
using System;
using YukiNet.Client;
using YukiNet.PackageParser;

namespace Assets.Scripts
{
    internal static class GameContext
    {
        static GameContext()
        {
            ConfigService = ConfigurationService.CreateInstance(s =>
            {
                s.AddSingleton<IPackageParser, PackageParser>();
                s.AddSingleton<NetworkConnection>();
                s.AddSingleton<ClientConnectionDispatcher>();
                s.AddSingleton<ClientManager>();
                s.AddSingleton<MenuManager>();
            });
        }

        public static ConfigurationService ConfigService
        {
            get;
        }

        public static IServiceProvider ServicesProvider => ConfigService.ServiceProvider;
    }
}