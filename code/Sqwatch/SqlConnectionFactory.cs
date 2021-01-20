using Microsoft.Data.SqlClient;
using System.Data;

namespace Sqwatch
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _config;

        public SqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Create()
        {
            var connection = new SqlConnection(_config.ConnectionString);
            connection.Open();

            return connection;
        }
    }
}
