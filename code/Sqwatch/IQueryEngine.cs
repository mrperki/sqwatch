using System;
using System.Collections.Generic;

namespace Sqwatch
{
    public interface IQueryEngine : IDisposable
    {
        IEnumerable<dynamic> ExecuteQuery();
    }
}
