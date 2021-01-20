using Sqwatch.Models;
using System;
using System.Data;

namespace Sqwatch
{
    public interface IConfiguration
    {
        string ConnectionString { get; }
        string Query { get; }
        TimeSpan ExecutionTime { get; }
        int QueryIntervalMs { get; }
        bool OutputToConsole { get; }
        bool OutputToFile { get; }
        string FileName { get; }
        ExistingFileOperation IfFileExists { get; }
        int QueryTimeoutSeconds { get; }
        IsolationLevel TransactionIsolationLevel { get; }
        void ApplyParameters(Parameters parameters);
    }
}
