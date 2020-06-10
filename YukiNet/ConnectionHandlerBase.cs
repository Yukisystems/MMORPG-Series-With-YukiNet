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

namespace YukiNet
{
    public abstract class ConnectionHandlerBase<T>
    {
        /// <summary>
        /// Handle Unknown packages
        /// </summary>
        /// <param name="connection">The COnnection</param>
        /// <param name="ParsedData">The parsedData</param>
        /// <param name="type">The type</param>
        protected abstract void HandleUnknownPackage(T connection, object ParsedData, UInt32 type);

        /// <summary>
        /// Invokes the handler
        /// </summary>
        /// <param name="connection">The COnnection</param>
        /// <param name="ParsedData">The parsedData</param>
        /// <param name="type">The type</param>
        public void InvokeAction(T connection, object ParsedData, UInt32 type)
        {
            foreach (var meth in GetType().GetMethods())
            {
                var attrib = meth.GetCustomAttribute<PackageHandlerAttribute>();
                if (attrib == null)
                {
                    continue;
                }

                if (attrib.Type == type)
                {
                    meth.Invoke(this, new object[] { connection, ParsedData });
                    return;
                }
            }

            HandleUnknownPackage(connection, ParsedData, type);
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class PackageHandlerAttribute : Attribute
    {
        public PackageHandlerAttribute(UInt32 type)
        {
            Type = type;
        }

        public UInt32 Type { get; }
    }
}