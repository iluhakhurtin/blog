using System;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class FilesRepository : Repository<File>, IFilesRepository
    {
        public FilesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<File> GetByNameAsync(string fileName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                ""Id"",
                                ""Name"",
                                ""MimeType"",
                                ""Extension"",
                                ""Data""
                                FROM ""Files""
                                WHERE ""Name""=:Name;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var nameParam = command.Parameters.AddWithValue("Name", fileName);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var file = new File();
                            file.Id = (Guid)dataReader["Id"];
                            file.Name = Convert.ToString(dataReader["Name"]);
                            file.MimeType = Convert.ToString(dataReader["MimeType"]);
                            file.Extension = Convert.ToString(dataReader["Extension"]);
                            file.Data = (byte[])dataReader["Data"];

                            return file;
                        }
                    }
                }
            }
            return null;
        }


        public async Task<File> GetByIdAsync(Guid fileId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                ""Name"",
                                ""MimeType"",
                                ""Extension"",
                                ""Data""
                                FROM ""Files""
                                WHERE ""Id""=:Id;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var idParam = command.Parameters.AddWithValue("Id", fileId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var file = new File();
                            file.Id = fileId;
                            file.Name = Convert.ToString(dataReader["Name"]);
                            file.MimeType = Convert.ToString(dataReader["MimeType"]);
                            file.Extension = Convert.ToString(dataReader["Extension"]);
                            file.Data = (byte[])dataReader["Data"];

                            return file;
                        }
                    }
                }
            }
            return null;
        }

        public async Task AddAsync(File file)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {

                string sql = String.Format(@"
                    INSERT INTO ""Files""(
                        ""Id"",
	                    ""Name"", 
	                    ""Extension"",
                        ""MimeType"",
	                    ""Data""
	                )
	                VALUES (
                        :Id,
	                    :Name, 
	                    :Extension,
                        :MimeType,
	                    :Data
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var id = command.CreateParameter();
                    id.Direction = System.Data.ParameterDirection.Input;
                    id.DbType = System.Data.DbType.Guid;
                    id.ParameterName = ":Id";
                    id.Value = file.Id;
                    command.Parameters.Add(id);

                    var fileName = command.CreateParameter();
                    fileName.Direction = System.Data.ParameterDirection.Input;
                    fileName.DbType = System.Data.DbType.String;
                    fileName.ParameterName = ":Name";
                    fileName.Value = file.Name;
                    command.Parameters.Add(fileName);

                    var extension = command.CreateParameter();
                    extension.Direction = System.Data.ParameterDirection.Input;
                    extension.DbType = System.Data.DbType.String;
                    extension.ParameterName = ":Extension";
                    extension.Value = file.Extension;
                    command.Parameters.Add(extension);

                    var mimeType = command.CreateParameter();
                    mimeType.Direction = System.Data.ParameterDirection.Input;
                    mimeType.DbType = System.Data.DbType.String;
                    mimeType.ParameterName = ":MimeType";
                    mimeType.Value = file.MimeType;
                    command.Parameters.Add(mimeType);

                    var data = command.CreateParameter();
                    data.Direction = System.Data.ParameterDirection.Input;
                    data.DbType = System.Data.DbType.Binary;
                    data.ParameterName = ":Data";
                    data.Value = file.Data;
                    command.Parameters.Add(data);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task UpdateAsync(File file)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    UPDATE ""Files"" SET
	                    ""Name"" = :Name, 
	                    ""Extension"" = :Extension,
                        ""MimeType"" = :MimeType
                    WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", file.Id);
                    command.Parameters.AddWithValue(":Name", file.Name);
                    command.Parameters.AddWithValue(":Extension", file.Extension);
                    command.Parameters.AddWithValue(":MimeType", file.MimeType);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    DELETE FROM ""Files""
                        WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
    }
}
