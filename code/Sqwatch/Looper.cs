using System;
using System.Threading;

namespace Sqwatch
{
    public class Looper : ILooper
    {
        private const string _logPrefix = "Querying (";
        private readonly char[] _spinner = new[] { '|', '/', '-', '\\', '|', '/', '-', '\\' };

        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly DateTime _cutOff;

        private int _iterationCount = 0;
        private int _diffCount = 0;        
        private int _spinIndex = -1;

        public Looper(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
            _cutOff = DateTime.Now + config.ExecutionTime;
        }

        public (int, int) Loop(Func<bool> func)
        {
            while (DateTime.Now < _cutOff)
            {
                Thread.Sleep(_config.QueryIntervalMs);

                if (func.Invoke()) _diffCount++;
                _iterationCount++;
                LogSpinner();
            }

            return (_iterationCount, _diffCount);
        }

        private void LogSpinner()
        {
            _logger.Log(_logPrefix + Spinner() + $"): {_diffCount} diffs");
        }

        private char Spinner()
        {
            _spinIndex = _spinIndex >= _spinner.Length ? 0 : _spinIndex + 1;
            return _spinner[_spinIndex];
        }
    }
}
