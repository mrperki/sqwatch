using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Sqwatch
{
    public class QueryEngine : IDisposable
    {
        private readonly SqlConnection _connection;

        public QueryEngine(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        public IEnumerable<dynamic> ExecuteQuery()
        {
            using var transaction = _connection.BeginTransaction(Configuration.TransactionIsolationLevel);
            return _connection.Query(Configuration.Query, transaction: transaction);
        }
    }
}
