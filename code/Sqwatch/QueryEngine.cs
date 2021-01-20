using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sqwatch
{
    public class QueryEngine : IQueryEngine
    {
        private readonly IConfiguration _config;
        private readonly IDbConnection _connection;

        public QueryEngine(IConfiguration config, IConnectionFactory factory)
        {
            _config = config;
            _connection = factory.Create();
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        public IEnumerable<dynamic> ExecuteQuery()
        {
            using var transaction = _connection.BeginTransaction(_config.TransactionIsolationLevel);
            return _connection.Query(_config.Query, transaction: transaction);
        }
    }
}
