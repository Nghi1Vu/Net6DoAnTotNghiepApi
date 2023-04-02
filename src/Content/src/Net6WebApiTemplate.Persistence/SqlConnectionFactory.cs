using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net6WebApiTemplate.Application.Shared.Interface;
using System.Data;

namespace Net6WebApiTemplate.Persistence
{
    public sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Net6WebApiConnection"));
        }
    }
}