using System;

namespace Sqwatch
{
    public interface ILooper
    {
        (int, int) Loop(Func<bool> func);
    }
}
