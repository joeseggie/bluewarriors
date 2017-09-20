using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Models;
using Bluewarriors.Mvc.Repository;

namespace BlueWarriors.Mvc.Repository
{
    public class LeaderTypeRepository<T> : IRepository<LeaderType> where T : LeaderType
    {
        private readonly IDatabaseConnection _databaseConnection;

        public LeaderTypeRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task AddAsync(LeaderType newItem)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"INSERT INTO LeaderType (Description) VALUES ('{newItem.Description}');";

                var command = db.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                try
                {
                    await db.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    // Log exception
                    throw;
                }
            }
        }

        public async Task<LeaderType> GetAsync(int itemId)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"SELECT TOP 1 LeaderTypeId, Description FROM LeaderType where LeaderTypeId = {itemId};";

                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                try
                {
                    await db.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if(reader.Read())
                        {
                            var leaderType = new LeaderType{
                                LeaderTypeId = reader.GetInt32(0),
                                Description = reader.GetString(1)
                            };

                            return leaderType;
                        }
                        else
                        {
                            // Log leader type not found.
                            return null;
                        }
                    }
                }
                catch (SqlException)
                {
                    // Log exception
                    throw;
                }
            }
        }

        public async Task<IEnumerable<LeaderType>> GetAsync()
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = "SELECT LeaderTypeId, Description FROM LeaderType;";

                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                try
                {
                    await db.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var leaderTypes = new List<LeaderType>();
                        
                        if(reader != null)
                        {
                            while (reader.Read())
                            {
                                var leaderType = new LeaderType{
                                    LeaderTypeId = reader.GetInt32(0),
                                    Description = reader.GetString(1)
                                };

                                leaderTypes.Add(leaderType);
                            }
                            
                            return leaderTypes;
                        }
                        else
                        {
                            // Log no leader types found.
                            return null;
                        }
                    }
                }
                catch (SqlException)
                {
                    // Log exception
                    throw;
                }
            }
        }

        public async Task UpdateAsync(LeaderType itemUpdate)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"UDPATE LeaderType SET Description = '{itemUpdate.Description}' WHERE LeaderTypeId = {itemUpdate.LeaderTypeId};";

                var command = db.CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                try
                {
                    await db.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    // Log exception
                    throw;
                }
            }
        }
    }
}