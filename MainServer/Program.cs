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
using System.Reflection;
using MainServer.Business;
using MainServer.Serivces;
using MainServer.Services;
using MainServer.Services.WebsocketServer;
using Microsoft.Extensions.DependencyInjection;
using YukiNet;
using YukiNet.Client;
using YukiNet.PackageParser;
using YukiNet.Server;

namespace MainServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ///var logger = ConfigurationService.Instance.ServiceProvider.GetService<ILogger<AuthService>>();
            var configServices = ConfigurationService.CreateInstance(serviceDescriptors =>
            {
                serviceDescriptors.AddPackageParser(new[] {Assembly.GetExecutingAssembly()});
                serviceDescriptors.AddServerPackageDispatcher();
                // Auth Service Handler
                serviceDescriptors.AddScoped<ConnectionHandlerBase<NetworkConnection>, ServerConnectionHandler>();
                serviceDescriptors.AddSingleton<IAuthStore, AuthStore>();
                serviceDescriptors.AddSingleton<NetworkService>();
                serviceDescriptors.AddSingleton<WorldserverRegistrationService>();
                // Dapper Service Handler
                serviceDescriptors.AddScoped<IUserRepository, UserRepository>();
                serviceDescriptors.AddScoped<ICharRepository, CharacterRepository>();
            });

            configServices.ServiceProvider.GetRequiredService<NetworkService>().Start();
            configServices.ServiceProvider.GetRequiredService<WorldserverRegistrationService>().Start();


            Console.ReadLine();
        }
    }
}