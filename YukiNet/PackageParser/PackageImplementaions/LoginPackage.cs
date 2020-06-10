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

namespace YukiNet.PackageParser.PackageImplementaions
{
    [PackageTypAttribute(CommunicationPackageValues.LOGIN_REQUEST)]
    public class LoginRequestPackage : PackageBase
    {
        public int ErrorMsg { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginRequestPackage() : base(CommunicationPackageValues.LOGIN_REQUEST)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            Username = reader.ReadString();
            Password = reader.ReadString();
        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(Username);
            writer.Write(Password);
        }
    }

    [PackageTypAttribute(CommunicationPackageValues.LOGIN_RESPONSE)]
    public class LoginResponsePackage : PackageBase
    {
        public bool IsValid { get; set; }
        public int userID { get; set; }
        public int ErrorMsg { get; set; }
        public LoginResponsePackage() : base(CommunicationPackageValues.LOGIN_RESPONSE)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();
            ErrorMsg = reader.ReadInt32();
            userID = reader.ReadInt32();
        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);         
            writer.Write(IsValid);
            writer.Write(ErrorMsg);
            writer.Write(userID);
        }
    }
}