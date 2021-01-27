namespace Sqwatch
{
    public interface ILogger
    {
        void Log(string message, bool inline = false);
    }
}
