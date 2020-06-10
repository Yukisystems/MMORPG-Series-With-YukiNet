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

using YukiNet.PackageParser.PackageImplementaions;

namespace MainServer.Models.User
{
    public class CharacterModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Charname { get; set; }
        public string UmaData { get; set; }


        public static explicit operator CharData(CharacterModel model)
        {
            return new CharData
            {
                CharId = model.Id,
                CharName = model.Charname,
                UmaData = model.UmaData
            };
        }

        public static explicit operator CharacterModel(CharData model)
        {
            return new CharacterModel
            {
                Id = model.CharId,
                Charname = model.CharName,
                UmaData = model.UmaData
            };
        }

        //public static CharacterModel operator +(CharacterModel model, CharacterModel model2)
        //{
        //    return null;
        //}


        //  public string Class { get; set; }
    }
}