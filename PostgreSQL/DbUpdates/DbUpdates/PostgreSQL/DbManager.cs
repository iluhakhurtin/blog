using DbUpdates.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdates.PostgreSQL
{
    class DbManager: DbBase, IDbManager
    {
        public DbManager(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<IList<string>> GetDatabasesAsync(string dbNameTemplate)
        {
            List<string> databases = new List<string>();
            using(NpgsqlConnection connection = new NpgsqlConnection(base.ConnectionString))
            {
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT 
    datname
    FROM pg_catalog.pg_database
    WHERE datname ILIKE @DbNameTemplate";

                    NpgsqlParameter parameter = command.CreateParameter();
                    parameter.ParameterName = "@DbNameTemplate";
                    parameter.Value = "%" + dbNameTemplate + "%";
                    command.Parameters.Add(parameter);

                    await connection.OpenAsync();

                    using (DbDataReader dbDataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dbDataReader.ReadAsync())
                        {
                            string dbName = Convert.ToString(dbDataReader["datname"]);
                            databases.Add(dbName);
                        }
                    }
                }
            }
            return databases;
        }
    }
}
