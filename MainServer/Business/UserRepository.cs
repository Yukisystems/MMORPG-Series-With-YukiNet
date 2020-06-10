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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using MainServer.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using YukiNet;

namespace MainServer.Business
{
    internal class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly IConfigurationRoot Configuration;

        private readonly ILogger<UserRepository> logger;

        public UserRepository(ILogger<UserRepository> logger, IConfigurationRoot Configuration)
        {
            this.Configuration = Configuration;
            this.logger = logger;
            _connectionString = Configuration.GetConnectionString("LocalMySQL");
        }

        private IDbConnection _connection => new MySqlConnection(_connectionString);

        public void AddUser(string User, string password, string email)
        {
            using (var dbConnection = _connection)
            {
                const string query = @"INSERT INTO Users (

                                    User,
                                    password,
                                    email
                                ) VALUES (

                                @UserName,
                                @Password,
                                @EmailAdress)";

                var result = dbConnection.Execute(query,
                    new {UserName = User, Password = Encryption.HashPassword(password), EmailAdress = email});
            }
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public List<UserModel> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public (bool success, IEnumerable<CharacterModel> charlist) GetCharacterList(int userId)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public (bool, int) PasswordOK(string username, string password)
        {
            using (var dbConnection = _connection)
            {
                const string query = @"SELECT * From user WHERE username=@username AND active = 1";

                dbConnection.Open();

                var User = dbConnection.Query<UserModel>(query, new {UserName = username}).FirstOrDefault();

                var validPassword = Encryption.ValidatePassword(password, User.Password);

                if (validPassword)
                {
                    logger.LogInformation("Password is Correct");

                    return (true, User.Id);
                }

                logger.LogInformation("Password is Incorrect");
                return (false, -1);
            }
        }

        public bool UserExists(string username)
        {
            using (var dbConnection = _connection)
            {
                const string query = @"SELECT * From user WHERE username=@username AND active = 1";

                dbConnection.Open();

                var User = dbConnection.Query<UserModel>(query, new {UserName = username}).FirstOrDefault();

                if (User == null)
                {
                    logger.LogInformation("UserName doesnt Exists");
                    return false;
                }

                logger.LogInformation("UserName Exists");
                return true;
            }
        }

        public void UpdateUser(UserModel User)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CharacterModel> GetCharactersByID(int UserID)
        {
            using (var dbConnection = _connection)
            {
                const string query = @"SELECT * FROM  characters WHERE UserId=@UserID;";

                dbConnection.Open();

                var Characters = dbConnection.Query<CharacterModel>(query, new {UserId = UserID});

                return Characters;
            }
        }
    }
}