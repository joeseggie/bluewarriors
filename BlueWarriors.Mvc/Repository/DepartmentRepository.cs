using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Models;
using BlueWarriors.Mvc.Repository;

namespace Bluewarriors.Mvc.Repository
{
    public class DepartmentRepository<T> : IRepository<Department> where T : Department
    {
        private readonly IDatabaseConnection _databaseConnection;

        public DepartmentRepository(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task AddAsync(Department newItem)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"INSERT INTO Department (Name) VALUES ('{newItem.Name}');";

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

        public async Task<Department> GetAsync(int itemId)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"SELECT TOP 1 DepartmentId, Name FROM Department where DepartmentId = {itemId};";

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
                            var department = new Department{
                                DepartmentId = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };

                            return department;
                        }
                        else
                        {
                            // Log no department found.
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

        public async Task<IEnumerable<Department>> GetAsync()
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = "SELECT DepartmentId, Name FROM Department;";

                var command = db.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                try
                {
                    await db.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var departments = new List<Department>();
                        
                        if(reader != null)
                        {
                            while (reader.Read())
                            {
                                var department = new Department{
                                    DepartmentId = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };

                                departments.Add(department);
                            }
                            
                            return departments;
                        }
                        else
                        {
                            // Log no departments returned.
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

        public async Task UpdateAsync(Department itemUpdate)
        {
            using (var db = _databaseConnection.GetConnection())
            {
                string query = $@"UDPATE Department SET Name = '{itemUpdate.Name}' WHERE DepartmentId = {itemUpdate.DepartmentId};";

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