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
    }
}
