using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Repository;
using BlueWarriors.Mvc.Models;

namespace BlueWarriors.Mvc.Repository
{
    public class RegionRepository<T> : IRepository<Region> where T: Region
    {
        private readonly IDatabaseConnection _databaseConnection;

        public RegionRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task AddAsync(Region newItem)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"INSERT INTO Region (Name) VALUES ('{newItem.Name}');";

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

        public async Task<Region> GetAsync(int itemId)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"SELECT TOP 1 RegionId, Name FROM Region where RegionId = {itemId};";

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
                            var region = new Region{
                                RegionId = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };

                            return region;
                        }
                        else
                        {
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

        public async Task<IEnumerable<Region>> GetAsync()
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = "SELECT RegionId, Name FROM Region;";

                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                try
                {
                    await db.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var regions = new List<Region>();
                        
                        if(reader != null)
                        {
                            while (reader.Read())
                            {
                                var region = new Region{
                                    RegionId = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };

                                regions.Add(region);
                            }
                            
                            return regions;
                        }
                        else
                        {
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

        public async Task UpdateAsync(Region itemUpdate)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"UDPATE Region SET Name = '{itemUpdate.Name}' WHERE RegionId = {itemUpdate.RegionId};";

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