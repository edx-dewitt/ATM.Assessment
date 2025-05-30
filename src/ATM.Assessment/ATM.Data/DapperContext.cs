using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace AccountService.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            // "DefaultConnection" should be in your appsettings.json or environment
            // For example: "Data Source=AccountsDb.db" (SQLite) or 
            // "Server=localhost;Database=AccountsDb;User Id=sa;Password=MySecret123" for SQL Server
            var dbLocation = Path.Combine(Path.GetTempPath(), "atm.db");
            var connectionString = $"Data Source={dbLocation}";
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
