using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using BlueWarriors.Services.Extensions;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BlueWarriors.Services
{
    public class Agent : IAgent
    {
        private readonly IConfigurationRoot _configuration;

        public int AgentId { get; set; }
        public string Name { get; set; }
        public int Msisdn { get; set; }
        public int LeaderId { get; set; }
        public int AgentTypeId { get; set; }
        public string Status{ get; set; }
        public byte[] RowVersion { get; set; }
        public string AuthPassword { get; set; }
        public DateTime? DeactivationDate { get; set; }

        public Agent()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Secret.json");

            _configuration = builder.Build();
        }

        public bool IsAuthenticated(string msisdn, string password)
        {
            using(var db = DatabaseConnection())
            {
                try
                {
                    db.Open();
                    var query = $"Select TOP 1 Msisdn from Agent where Msisdn = {msisdn} and AuthPassword = {password};";
                    var command = db.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    var result = (string)command.ExecuteScalar();

                    if(result == msisdn)
                    {
                        return true;
                    }

                    return false;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public IEnumerable<Agent> AgentsList()
        {
            var db = DatabaseConnection();
            try
                {
                    db.Open();
                    var query = "Select AgentId, Name, Msisdn, LeaderId, AgentTypeId, Status, RowVersion, AuthPassword from Agent where DeactivationDate is NULL and Status = 'ACTIVE';";
                    var command = db.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    ;

                    var agentsList = new List<Agent>();
                    using(var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            agentsList.Add(new Agent{
                                AgentId = dataReader.GetInt32(0),
                                Name = dataReader.GetNullableString(1),
                                Msisdn = dataReader.GetInt32(2),
                                LeaderId = dataReader.GetInt32(3),
                                AgentTypeId = dataReader.GetInt32(4),
                                Status = dataReader.GetNullableString(5),
                                RowVersion = dataReader.GetFieldValue<byte[]>(6),
                                AuthPassword = dataReader.GetNullableString(7)
                            });
                        }
                    }
                    return agentsList;
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                    db.Close();
                }
        }

        public int AddAgent(Agent newAgent)
        {
            var db = DatabaseConnection();
            try
            {
                db.Open();
                var query = $"Insert into Agent (Name, Msisdn, LeaderId, AgentTypeId, Status, AuthPassword) OUTPUT INSERTED.AgentId VALUES ('{newAgent.Name}', {newAgent.Msisdn}, 132, 1, 'ACTIVE', '{newAgent.AuthPassword}');";
                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                var agentId = (int)command.ExecuteScalar();
                return agentId;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                db.Dispose();
                db.Close();
            }
        }

        public Agent GetAgent(int agentId)
        {
            var db = DatabaseConnection();
            try
                {
                    db.Open();
                    var query = $"Select TOP 1 AgentId, Name, Msisdn, LeaderId, AgentTypeId, Status, RowVersion, AuthPassword from Agent where AgentId = {agentId};";
                    var command = db.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    
                    using(var dataReader = command.ExecuteReader())
                    {
                        var agentProfile = new Agent();
                        while (dataReader.Read())
                        {
                            agentProfile.AgentId = dataReader.GetInt32(0);
                            agentProfile.Name = dataReader.GetNullableString(1);
                            agentProfile.Msisdn = dataReader.GetInt32(2);
                            agentProfile.LeaderId = dataReader.GetInt32(3);
                            agentProfile.AgentTypeId = dataReader.GetInt32(4);
                            agentProfile.Status = dataReader.GetNullableString(5);
                            agentProfile.RowVersion = dataReader.GetFieldValue<byte[]>(6);
                            agentProfile.AuthPassword = dataReader.GetNullableString(7);
                        }
                        return agentProfile;
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                    db.Close();
                }
        }

        public int UpdateAgent(Agent agentDetails)
        {
            var db = DatabaseConnection();
            try
            {
                db.Open();
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"Update Agent set Name = '{agentDetails.Name}', Msisdn = {agentDetails.Msisdn}, Status = '{agentDetails.Status}'");
                if(agentDetails.AuthPassword != string.Empty && !string.IsNullOrWhiteSpace(agentDetails.AuthPassword))
                    queryBuilder.Append($", AuthPassword = '{agentDetails.AuthPassword}' ");
                queryBuilder.Append($"where AgentId = {agentDetails.AgentId}");
                var query = queryBuilder.ToString();
                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                command.ExecuteNonQuery();
                
                return agentDetails.AgentId;
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                db.Dispose();
                db.Close();
            }
        }

        public List<Agent> DeactivatedAgentsList()
        {
            var db = DatabaseConnection();
            try
                {
                    db.Open();
                    var query = "Select AgentId, Name, Msisdn, LeaderId, AgentTypeId, Status, RowVersion, AuthPassword, DeactivationDate from Agent where DeactivationDate IS NOT NULL and Status = 'DEACTIVATED';";
                    var command = db.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;
                    ;

                    var agentsList = new List<Agent>();
                    using(var dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            agentsList.Add(new Agent(){
                                AgentId = dataReader.GetInt32(0),
                                Name = dataReader.GetNullableString(1),
                                Msisdn = dataReader.GetInt32(2),
                                LeaderId = dataReader.GetInt32(3),
                                AgentTypeId = dataReader.GetInt32(4),
                                Status = dataReader.GetNullableString(5),
                                RowVersion = dataReader.GetFieldValue<byte[]>(6),
                                AuthPassword = dataReader.GetNullableString(7),
                                DeactivationDate = dataReader.GetNullableDateTime(8)
                            });
                        }
                    }
                    return agentsList;
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                    db.Close();
                }
        }

        public void DeactivateAgent(int agentId)
        {
            var db = DatabaseConnection();
            try
                {
                    db.Open();
                    var query = $"UPDATE Agent SET DeactivationDate = '{DateTime.Now.ToString("yyyy-MM-dd")}', Status = 'DEACTIVATED' where AgentId = {agentId};";
                    var command = db.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;

                    command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
                finally
                {
                    db.Dispose();
                    db.Close();
                }
        }

        private SqlConnection DatabaseConnection()
        {
            var server = _configuration["server"];
            var database = _configuration["database"];
            var username = _configuration["username"];
            var password = _configuration["password"];

            return new SqlConnection($"Data Source={server};Initial Catalog={database};User Id={username};Password={password};");
        }
    }
}
