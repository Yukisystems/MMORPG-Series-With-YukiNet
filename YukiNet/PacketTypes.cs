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

namespace YukiNet
{
    public static class CommunicationPackageValues
    {
        public const UInt32 LOGIN_REQUEST = 0x0001;
        public const UInt32 LOGIN_RESPONSE = 0x0002;
        public const UInt32 REALM_REQUEST = 0x0003;
        public const UInt32 REALM_RESPONSE = 0x0004;
        public const UInt32 CHARACTER_CREATION_REQUEST = 0x0005;
        public const UInt32 CHARACTER_CREATION_RESPONSE = 0x0006;
        public const UInt32 CHARACTER_SELECTION_REQUEST = 0x0007;
        public const UInt32 CHARACTER_SELECTION_RESPONSE = 0x0008;

        public const UInt32 KEEPALIVE = 0xFFFE;
        public const UInt32 ERROR = 0xFFFF;
    }
}