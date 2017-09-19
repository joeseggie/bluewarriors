using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BlueWarriors.Mvc.Repository
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SqlConnection GetConnection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Secret.json");

            var _configuration = builder.Build();

            var server = _configuration["server"];
            var database = _configuration["database"];
            var username = _configuration["username"];
            var password = _configuration["password"];

            return new SqlConnection($"Data Source={server};Initial Catalog={database};User Id={username};Password={password};");
        }
    }
}