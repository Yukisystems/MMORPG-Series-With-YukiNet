﻿///////////////////////////////////////////////////////////////////////////////////////
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

namespace YukiNet.PackageParser
{
    public abstract class PackageBase
    {
        public UInt32 ID { get; }

        public PackageBase(UInt32 ID)
        {
            this.ID = ID;
        }

        /// <summary>
        /// writes the data to stream - with package ID !
        /// </summary>
        /// <param name="writer"></param>
        public abstract void SerialzeToStream(BinaryWriter writer);

        /// <summary>
        /// Reads the data from the stream -> the ID should be already read from stream
        /// </summary>
        /// <param name="reader"></param>
        public abstract void DeserialzeFromStream(BinaryReader reader);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PackageTypAttributeAttribute : Attribute
    {
        public UInt32 PackageType { get; }

        public PackageTypAttributeAttribute(UInt32 type) => PackageType = type;
    }
}