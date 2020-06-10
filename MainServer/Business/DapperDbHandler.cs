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

using Microsoft.Extensions.Logging;

namespace MainServer.Business
{
    internal class DapperDbHandler : IDbHandler
    {
        private readonly ILogger<DapperDbHandler> logger;

        public DapperDbHandler(ILogger<DapperDbHandler> logger)
        {
            this.logger = logger;
        }

        public void Test()
        {
            logger.LogInformation("TEST ");
        }
    }
}