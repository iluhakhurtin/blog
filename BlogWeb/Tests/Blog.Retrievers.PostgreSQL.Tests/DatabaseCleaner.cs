using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Respawn;

namespace Blog.Retrievers.PostgreSQL.Tests
{
    public class DatabaseCleaner
    {
        static DatabaseCleaner()
        {
            
        }
        
        public async static Task CleanUp(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var checkpoint = new Checkpoint()
                {
                    SchemasToInclude = new[]
                    {
                        "public"
                    },
                    TablesToIgnore = new[]
                    {
                        "DbSettings"
                    },
                    DbAdapter = DbAdapter.Postgres
                };

                await checkpoint.Reset(connection);
            }
        }
    }
}
