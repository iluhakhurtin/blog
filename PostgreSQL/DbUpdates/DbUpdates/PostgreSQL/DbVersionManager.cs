using DbUpdates.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdates.PostgreSQL
{
    class DbVersionManager : DbBase, IDbVersionManager
    {
        private string dbName;

        public DbVersionManager(string dbName, string connectionString)
            : base(connectionString)
        {
            this.dbName = dbName;
        }

        private async Task CreateVersions(string versionFieldName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.ConnectionString))
            {
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                                            CREATE TABLE public.""DbSettings""
                                            (
                                                """ + versionFieldName + @""" integer NULL
                                            )
                                            WITH(
                                                OIDS = FALSE
                                            )
                                            TABLESPACE pg_default;
                                            ";

                    await connection.OpenAsync();
                    connection.ChangeDatabase(this.dbName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task InsertNullVersion(string versionFieldName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.ConnectionString))
            {
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO public.""DbSettings""(""" + versionFieldName + @""") VALUES(NULL);";

                    await connection.OpenAsync();
                    connection.ChangeDatabase(this.dbName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int?> GetVersionAsync(string versionFieldName)
        {
            if (String.IsNullOrEmpty(versionFieldName))
                throw new ArgumentException("versionFileName cannot be nul or empty. Provide appropriate version field name.");

            int? dbVersion = null;
            using (NpgsqlConnection connection = new NpgsqlConnection(base.ConnectionString))
            {
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT """ + versionFieldName + @""" FROM ""DbSettings"";";

                    await connection.OpenAsync();
                    connection.ChangeDatabase(this.dbName);
                    try
                    {
                        using (DbDataReader dbDataReader = await command.ExecuteReaderAsync())
                        {
                            if(await dbDataReader.ReadAsync())
                            {
                                if(await dbDataReader.IsDBNullAsync(0))
                                {
                                    dbVersion = null;
                                }
                                else
                                {
                                    dbVersion = Convert.ToInt32(dbDataReader[versionFieldName]);
                                }
                            }
                        }
                    }
                    catch(PostgresException)
                    {
                        connection.Close();
                        await this.CreateVersions(versionFieldName);
                        await this.InsertNullVersion(versionFieldName);
                    }
                }
            }
            return dbVersion;
        }

        public async Task SetVersionAsync(int version, string versionFieldName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.ConnectionString))
            {
                using (NpgsqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE ""DbSettings"" SET """ + versionFieldName + @"""=@DbVersion;";

                    NpgsqlParameter parameter = command.CreateParameter();
                    parameter.ParameterName = "@DbVersion";
                    parameter.Value = version;
                    command.Parameters.Add(parameter);

                    await connection.OpenAsync();
                    connection.ChangeDatabase(this.dbName);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
