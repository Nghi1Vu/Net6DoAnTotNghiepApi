using System.Data;

namespace Net6WebApiTemplate.Application.Shared.Interface
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}