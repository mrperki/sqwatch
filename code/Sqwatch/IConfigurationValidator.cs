using Sqwatch.Models;

namespace Sqwatch
{
    public interface IConfigurationValidator
    {
        string Validate(Parameters parameters);
    }
}