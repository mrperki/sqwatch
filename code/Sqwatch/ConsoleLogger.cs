using System;

namespace Sqwatch
{
    public class ConsoleLogger : ILogger
    {
        private readonly bool _shouldLog;

        public ConsoleLogger(IConfiguration config)
        {
            _shouldLog = !config.OutputToConsole;
        }

        public void Log(string message, bool inline = false)
        {
            if (!_shouldLog) return;

            if (inline)
                Console.Write(message);
            else
                Console.WriteLine(message);
        }
    }
}
