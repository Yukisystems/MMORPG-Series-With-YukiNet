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
using System.IO;

namespace YukiNet.PackageParser.PackageImplementaions
{
    //[PackageTypAttribute(CommunicationPackage.CHAR_REQUEST)]
    //   public class CharacterCreationRequestPackage
    //    {
    //    }
    [PackageTypAttribute(CommunicationPackageValues.REALM_REQUEST)]
    public class RealmRequestPackage : PackageBase
    {
        public int RealmID { get; set; }
        public int ErrorMsg { get; set; }
        public RealmRequestPackage() : base(CommunicationPackageValues.REALM_REQUEST)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            // Bei Ints immer ReadInt32 nutzen
            RealmID = reader.ReadInt32();
        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(RealmID);
        }
    }

    [PackageTypAttribute(CommunicationPackageValues.REALM_RESPONSE)]
    public class RealmResponsePackage : PackageBase
    {
        // public bool IsValid { get; set; }
        public int RealmID { get; set; }
        public int ErrorMsg { get; set; }
        public RealmResponsePackage() : base(CommunicationPackageValues.REALM_RESPONSE)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            RealmID = reader.ReadInt32();
        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(RealmID);
            // writer.Write(IsValid);
        }
    }
}