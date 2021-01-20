using System.Data;

namespace Sqwatch
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}
