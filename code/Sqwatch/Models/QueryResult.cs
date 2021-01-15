using System;
using System.Collections.Generic;

namespace Sqwatch.Models
{
    public enum ResultType
    {
        Full,
        Diff,
        Empty
    }

    public enum ResultStatus
    {
        Undefined,
        Ok,
        Error
    }

    public class QueryResult
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public ResultType Type { get; set; } = ResultType.Empty;
        public ResultStatus Status { get; set; } = ResultStatus.Undefined;
        public int ExecutionTimeMs { get; set; }
        public int RowCount { get; set; } = 0;
        public IEnumerable<dynamic> Result { get; set; }
    }
}
