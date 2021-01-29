using System;

namespace Sqwatch.UnitTests.Mocks
{
    public class MockLooper : ILooper
    {
        public int IterationCount { get; set; }
        public int DiffCount { get; set; }

        public (int, int) Loop(Func<bool> func)
        {
            return (IterationCount, DiffCount);
        }
    }
}
