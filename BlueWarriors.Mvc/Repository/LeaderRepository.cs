using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Repository;
using Bluewarriors.Mvc.Models;

namespace BlueWarriors.Mvc.Repository
{
    public class LeaderRepository<T> : IRepository<Leader>
    {
        private readonly IDatabaseConnection _databaseConnection;

        public LeaderRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task AddAsync(Leader newItem)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"INSERT INTO Leader (Name, Msisdn, RegionId, DepartmentId, LeaderTypeId, Status) VALUES ('{newItem.Name}', {newItem.Msisdn}, {newItem.RegionId}, {newItem.DepartmentId}, {newItem.LeaderTypeId}, 'ACTIVE');";

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

        public async Task<Leader> GetAsync(int itemId)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"SELECT TOP 1 l.LeaderId, l.Name, l.Msisdn, l.RegionId, r.Name AS Region, l.DepartmentId, d.Name AS Department, l.LeaderTypeId, t.Description AS LeaderType, l.Status, l.DeactivationDate FROM Leader l LEFT JOIN Region r ON l.RegionId = r.RegionId LEFT JOIN Department d ON L.DepartmentId = d.DepartmentId LEFT JOIN LeaderType t ON l.LeaderTypeId = t.LeaderTypeId where l.LeaderId = {itemId};";

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
                            var leader = new Leader{
                                LeaderId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Msisdn = reader.GetInt32(2),
                                RegionId = reader.GetInt32(3),
                                Region = reader.GetString(4),
                                DepartmentId = reader.GetInt32(5),
                                Department = reader.GetString(6),
                                LeaderTypeId = reader.GetInt32(7),
                                LeaderType = reader.GetString(8),
                                Status = reader.GetString(9),
                                DeactivationDate = reader.GetDateTime(10)
                            };

                            return leader;
                        }
                        else
                        {
                            // Log no leader found.
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

        public async Task<System.Collections.Generic.IEnumerable<Leader>> GetAsync()
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = "SELECT RegionId, Name FROM RegionSELECT TOP 1 l.LeaderId, l.Name, l.Msisdn, l.RegionId, r.Name AS Region, l.DepartmentId, d.Name AS Department, l.LeaderTypeId, t.Description AS LeaderType, l.Status, l.DeactivationDate FROM Leader l LEFT JOIN Region r ON l.RegionId = r.RegionId LEFT JOIN Department d ON L.DepartmentId = d.DepartmentId LEFT JOIN LeaderType t ON l.LeaderTypeId = t.LeaderTypeId;";

                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                try
                {
                    await db.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var leaders = new List<Leader>();
                        
                        if(reader != null)
                        {
                            while (reader.Read())
                            {
                                var leader = new Leader{
                                    LeaderId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Msisdn = reader.GetInt32(2),
                                    RegionId = reader.GetInt32(3),
                                    Region = reader.GetString(4),
                                    DepartmentId = reader.GetInt32(5),
                                    Department = reader.GetString(6),
                                    LeaderTypeId = reader.GetInt32(7),
                                    LeaderType = reader.GetString(8),
                                    Status = reader.GetString(9),
                                    DeactivationDate = reader.GetDateTime(10)
                                };

                                leaders.Add(leader);
                            }
                            
                            return leaders;
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

        public async Task UpdateAsync(Leader itemUpdate)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"UDPATE Leader SET Name = '{itemUpdate.Name}', RegionId = {itemUpdate.RegionId}, DepartmentId = {itemUpdate.DepartmentId}, LeaderTypeId = {itemUpdate.LeaderTypeId} WHERE LeaderId = {itemUpdate.LeaderId} and Msisdn = {itemUpdate.Msisdn};";

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