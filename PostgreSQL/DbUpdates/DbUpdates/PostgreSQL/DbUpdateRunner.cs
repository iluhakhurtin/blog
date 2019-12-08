using DbUpdates.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdates.PostgreSQL
{
    class DbUpdateRunner : DbBase, IDbUpdateRunner
    {
        private string dbName;

        public DbUpdateRunner(string dbName, string connectionString)
            : base(connectionString)
        {
            this.dbName = dbName;
        }

        public async Task RunUpdateAsync(string updateContent)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.ConnectionString))
            {
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = updateContent;

                    await connection.OpenAsync();
                    connection.ChangeDatabase(this.dbName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
