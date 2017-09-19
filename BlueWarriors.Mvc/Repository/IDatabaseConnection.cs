using System.Data.SqlClient;

namespace BlueWarriors.Mvc.Repository
{
    public interface IDatabaseConnection
    {
        SqlConnection GetConnection();
    }
}