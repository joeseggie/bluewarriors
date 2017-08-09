using System;
using System.Data.SqlClient;

namespace BlueWarriors.Services.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static DateTime? GetNullableDateTime(this SqlDataReader reader, int columnIndex)
        {
            if(!reader.IsDBNull(columnIndex))
                return reader.GetDateTime(columnIndex);
            return null;
        }
        
        public static string GetNullableString(this SqlDataReader reader, int columnIndex)
        {
            if(!reader.IsDBNull(columnIndex))
                return reader.GetString(columnIndex);
            return string.Empty;
        }

        public static int? GetNullableInt32(this SqlDataReader reader, int columnIndex)
        {
            if(!reader.IsDBNull(columnIndex))
                return reader.GetInt32(columnIndex);
            return null;
        }
    }
}