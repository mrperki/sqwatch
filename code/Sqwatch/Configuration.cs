using Microsoft.Extensions.Configuration;
using Sqwatch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sqwatch
{
    internal static class Configuration
    {
        private static readonly IDictionary<string, string> _namedConnections;
        private static readonly IDictionary<string, string> _namedQueries;
        private static readonly ConfigDefaults _defaults;
        private static readonly ConfigSqlSettings _sqlSettings;
        
        private static Parameters _parameters;

        public static string ConnectionString { get; private set; }
        public static string Query { get; private set; }
        public static TimeSpan ExecutionTime { get; private set; }
        public static int QueryIntervalMs => (int?)_parameters?.QueryIntervalMs ?? _defaults.QueryIntervalMs;
        public static bool OutputToConsole => _parameters?.OutputToConsole ?? _defaults.OutputToConsole;
        public static bool OutputToFile => !string.IsNullOrEmpty(_parameters?.OutputToFile) || _defaults.OutputToFile;
        public static string FileName => _parameters?.OutputToFile ?? _defaults.FileName;
        public static ExistingFileOperation IfFileExists => _parameters?.IfFileExists ?? _defaults.IfFileExists;
        public static int QueryTimeoutSeconds => _sqlSettings.QueryTimeoutSeconds;
        public static IsolationLevel TransactionIsolationLevel => _sqlSettings.TransactionIsolationLevel;

        static Configuration()
        {
            var configRoot = new ConfigurationBuilder()
                .AddJsonFile("sqwatch.config.json", false, false)
                .Build();

            _namedConnections = configRoot.GetSection("namedConnections")
                .Get<List<NamedConnection>>()
                .ToDictionary(c => c.Name, c => c.ConnectionString);

            _namedQueries = configRoot.GetSection("namedQueries")
                .Get<List<NamedQuery>>()
                .ToDictionary(q => q.Name, q => q.Query);

            _defaults = configRoot.GetSection("defaults").Get<ConfigDefaults>();
            _sqlSettings = configRoot.GetSection("sqlSettings").Get<ConfigSqlSettings>();

            ConnectionString = string.Empty;
            Query = string.Empty;
            ExecutionTime = new TimeSpan(0, _defaults.MaxExecutionSeconds, 0);
        }

        public static bool ValidateConnectionName(string connectionName) => _namedConnections.ContainsKey(connectionName);

        public static bool ValidateQueryName(string queryName) => _namedQueries.ContainsKey(queryName);

        public static void ApplyParameters(Parameters parameters)
        {
            _parameters = parameters;

            ConnectionString = !string.IsNullOrEmpty(_parameters.ConnectionString)
                ? _parameters.ConnectionString
                : !string.IsNullOrEmpty(_parameters.ConnectionName) && ValidateConnectionName(_parameters.ConnectionName)
                    ? _namedConnections[_parameters.ConnectionName]
                    : _namedConnections.FirstOrDefault().Value;

            Query = !string.IsNullOrEmpty(_parameters.QuerySql)
                ? _parameters.QuerySql
                : !string.IsNullOrEmpty(_parameters.QueryName) && ValidateQueryName(_parameters.QueryName)
                    ? _namedQueries[_parameters.QueryName]
                    : _namedQueries.FirstOrDefault().Value;

            ExecutionTime = _parameters.RunForSeconds.HasValue || _parameters.RunForMinutes.HasValue
                ? new TimeSpan(0, (int?)_parameters.RunForMinutes ?? 0, (int?)_parameters.RunForSeconds ?? 0)
                : new TimeSpan(0, _defaults.MaxExecutionSeconds, 0);
        }
    }
}
