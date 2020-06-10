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
using System.Linq;
using MainServer.Business;
using MainServer.Services;
using Microsoft.Extensions.Logging;
using YukiNet;
using YukiNet.Client;
using YukiNet.PackageParser;
using YukiNet.PackageParser.PackageImplementaions;

namespace MainServer
{
    public class ServerConnectionHandler : ConnectionHandlerBase<NetworkConnection>
    {
        private readonly IAuthStore authStore;
        private readonly ICharRepository CharacterRepo;
        private readonly ILogger<ServerConnectionHandler> logger;
        private readonly IPackageParser packageParser;
        private readonly IUserRepository UserRepo;

        public ServerConnectionHandler(ILogger<ServerConnectionHandler> logger,
            IPackageParser packageParser,
            IUserRepository UserRepo,
            ICharRepository CharacterRepo,
            IAuthStore authStore)
        {
            this.logger = logger;
            this.packageParser = packageParser;
            this.UserRepo = UserRepo;
            this.CharacterRepo = CharacterRepo;
            this.authStore = authStore;
        }

        protected override void HandleUnknownPackage(NetworkConnection connection, object ParsedData, uint type)
        {
            throw new NotImplementedException();
        }

        [PackageHandler(CommunicationPackageValues.LOGIN_REQUEST)]
        public void HandleLogin(NetworkConnection connection, LoginRequestPackage parsedObjectData)
        {
            logger.LogInformation("LOGIN REQUEST");
            logger.LogDebug($"LOGIN DATA USER:{parsedObjectData.Username} PW: {parsedObjectData.Password}");

            //UserRepository repo = new UserRepository();
            if (!UserRepo.UserExists(parsedObjectData.Username))
            {
                packageParser.ParsePackgeToStream(new LoginResponsePackage
                {
                    //ToDO Sent ErrorMessageType Error
                    ErrorMsg = ErrorMsgTypes.Failed,
                    IsValid = false
                }, connection.Writer);
                return;
            }

            var (passwordOK, userId) = UserRepo.PasswordOK(parsedObjectData.Username, parsedObjectData.Password);
            if (!passwordOK)
            {
                packageParser.ParsePackgeToStream(new LoginResponsePackage
                {
                    //ToDO Sent ErrorMessageType Error
                    ErrorMsg = ErrorMsgTypes.Failed,
                    IsValid = false
                }, connection.Writer);
                //password was not correct
                logger.LogInformation("Wrong Password");
            }
            else
            {
                authStore.Add(connection.ConnectionId, userId);
                packageParser.ParsePackgeToStream(new LoginResponsePackage
                {
                    //ToDO Sent ErrorMessageType Successfull
                    ErrorMsg = ErrorMsgTypes.Success,
                    IsValid = true
                }, connection.Writer);
            }
        }

        [PackageHandler(CommunicationPackageValues.ERROR)]
        public void HandleError(NetworkConnection connection, object parsedObjectData)
        {
        }

        [PackageHandler(CommunicationPackageValues.REALM_REQUEST)]
        public void HandleRaceSelection(NetworkConnection connection, RealmRequestPackage parsedObjectData)
        {
            logger.LogInformation("Race REQUEST");
            logger.LogDebug($"Race Selected:{parsedObjectData.RealmID} ");

            //UserRepository repo = new UserRepository();

            packageParser.ParsePackgeToStream(new RealmResponsePackage
            {
                RealmID = parsedObjectData.RealmID
            }, connection.Writer);
        }

        [PackageHandler(CommunicationPackageValues.CHARACTER_CREATION_REQUEST)]
        public void HandleCharCreation(NetworkConnection connection, CharacterCreationRequestPackage parsedObjectData)
        {
            logger.LogInformation($"Char creation REQUEST from {authStore[connection.ConnectionId]}");
            // logger.LogDebug($"LOGIN DATA USER:{parsedObjectData.} PW: {parsedObjectData.Password}");
            if (CharacterRepo.CharExists(parsedObjectData.CharName))
            {
                logger.LogInformation("CharCreation: Something went wrong");
                packageParser.ParsePackgeToStream(new CharacterResponsePackage
                {
                    //ToDO Sent ErrorMessageType Error
                    IsValid = false,
                    ErrorMsg = ErrorMsgTypes.Failed
                    //NameDuplicate = false,
                    //NameContentsBadwords = false,
                    //    userID = userId
                }, connection.Writer);
            }
            else
            {
                //ToDO Sent ErrorMessageType Success
                CharacterRepo.SaveCharacterToDb(authStore[connection.ConnectionId], parsedObjectData.CharName,
                    parsedObjectData.UmaRecipe);

                logger.LogInformation("Character was created successfully");
                packageParser.ParsePackgeToStream(new CharacterResponsePackage
                {
                    IsValid = true,
                    ErrorMsg = ErrorMsgTypes.Success
                    //NameDuplicate = false,
                    //NameContentsBadwords = false,
                    //    userID = userId
                }, connection.Writer);
            }

            //packageParser.ParsePackgeToStream(new CharacterResponsePackage
            //{
            //    IsValid = true,
            //    //NameDuplicate = false,
            //    //NameContentsBadwords = false,
            //    //    userID = userId
            //}, connection.Writer);
        }

        [PackageHandler(CommunicationPackageValues.CHARACTER_SELECTION_REQUEST)]
        public void HandleCharSelection(NetworkConnection connection, CharacterSelectionRequestPackage parsedObjectData)
        {
            logger.LogInformation($"Char creation REQUEST from {authStore[connection.ConnectionId]}");
            // logger.LogDebug($"LOGIN DATA USER:{parsedObjectData.} PW: {parsedObjectData.Password}");

            var characters = UserRepo.GetCharactersByID(authStore[connection.ConnectionId]);


            //foreach(CharacterModel chars in characters)
            //{
            //    chars.Id;

            //}


            packageParser.ParsePackgeToStream(new CharacterSelectionResponsePackage
            {
                IsValid = true,
                ErrorMsg = ErrorMsgTypes.Success,
                Characters = characters.Select(x => (CharData) x)
                //Characters = characters,
                //NameDuplicate = false,
                //NameContentsBadwords = false,
                //    userID = userId
            }, connection.Writer);
        }
    }
}