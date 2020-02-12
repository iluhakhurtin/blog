using System.Threading.Tasks;
using Npgsql;
using Respawn;

namespace Blog.PostgreSQL.Tests
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
