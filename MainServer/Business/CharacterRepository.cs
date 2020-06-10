using System;
using System.Data;
using System.Linq;
using Dapper;
using MainServer.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace MainServer.Business
{
    internal class CharacterRepository : ICharRepository
    {
        private readonly string _connectionString;
        private readonly IConfigurationRoot Configuration;

        private readonly ILogger<CharacterRepository> logger;

        public CharacterRepository(ILogger<CharacterRepository> logger, IConfigurationRoot Configuration)
        {
            this.Configuration = Configuration;
            this.logger = logger;
            _connectionString = Configuration.GetConnectionString("LocalMySQL");
        }

        private IDbConnection _connection => new MySqlConnection(_connectionString);

        public bool CharExists(string charname)
        {
            using (var dbConnection = _connection)
            {
                const string query = @"SELECT COUNT(*) FROM  characters WHERE Charname=@charname;";

                dbConnection.Open();

                var Character = dbConnection.Query<CharacterModel>(query, new {Charname = charname})
                    .FirstOrDefault(); // .FirstOrDefault() new { Charname = charname }


                if (Character == null || Character.Charname != charname)
                {
                    logger.LogInformation("Character Name doesn't Exists");
                    return false;
                }

                logger.LogInformation("Charactername exists");
                return true;
            }
        }

        public void SaveCharacterToDb(int UserId, string charname, string UmaData)
        {
            using (var dbConnection = _connection)
            {
                const string query = @"INSERT INTO characters (

                                    UserId,
                                    Charname,
                                    UmaData
                                ) VALUES (

                                @UserId,
                                @charname,
                                @UmaData)";

                var result = dbConnection.Execute(query, new {UserId, Charname = charname, UmaData});
            }
        }

        public void UpdateUser(int UserID, string charname)
        {
            throw new NotImplementedException();
        }
    }
}