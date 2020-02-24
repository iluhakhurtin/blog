using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Retrievers.Article;
using Blog.Retrievers.File;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL
{
    internal class FilesRetriever : Retriever, IFilesRetriever
    {
        public FilesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<FilesPagedDataTable> GetFilesPagedAsync(
            string nameFilter,
            string extensionFilter,
            string mimeTypeFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize)
        {
            var filesPagedDataTable = new FilesPagedDataTable(pageNumber, pageSize);

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                    COUNT(1) OVER() AS ""ResultsCount"",
                                    f.""Id""        AS ""Id"",
                                    f.""Name""      AS ""Name"",
                                    f.""Extension"" AS ""Extension"",
                                    f.""MimeType""  AS ""MimeType""
                                    FROM ""Files"" f
                                    WHERE :NameFilter IS NULL OR f.""Name"" ILIKE('%' || :NameFilter || '%')
                                        AND :ExtensionFilter IS NULL OR f.""Extension"" ILIKE('%' || :ExtensionFilter || '%')
                                        AND :MimeTypeFilter IS NULL OR f.""MimeType"" ILIKE('%' || :MimeTypeFilter || '%')
                                    ORDER BY
                                        CASE
                                            WHEN :SortOrder = 'desc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'Name' THEN CAST(""Name"" AS text)
                                                    WHEN :SortColumn = 'Extension' THEN CAST(""Extension"" AS text)
                                                    WHEN :SortColumn = 'MimeType' THEN CAST(""MimeType"" AS text)
					                                ELSE CAST(""Id"" AS text)
				                                END
                                        END DESC,
		                                CASE
                                            WHEN :SortOrder = 'asc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'Name' THEN CAST(""Name"" AS text)
                                                    WHEN :SortColumn = 'Extension' THEN CAST(""Extension"" AS text)
                                                    WHEN :SortColumn = 'MimeType' THEN CAST(""MimeType"" AS text)
					                                ELSE CAST(""Id"" AS text)
				                                END
                                        END ASC
                                    LIMIT :PageSize
                                    OFFSET(:PageNumber - 1) * :PageSize;
                                    ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var filterParam = new NpgsqlParameter<string>("NameFilter", nameFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("ExtensionFilter", extensionFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("MimeTypeFilter", mimeTypeFilter);
                    command.Parameters.Add(filterParam);

                    var sortColumnParam = new NpgsqlParameter<string>("SortColumn", sortColumn);
                    command.Parameters.Add(sortColumnParam);

                    command.Parameters.AddWithValue("SortOrder", sortOrder);
                    command.Parameters.AddWithValue("PageNumber", pageNumber);
                    command.Parameters.AddWithValue("PageSize", pageSize);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (dataReader.Read())
                        {
                            if (filesPagedDataTable.TotalResultsCount == 0)
                                filesPagedDataTable.TotalResultsCount = Convert.ToInt32(dataReader["ResultsCount"]);

                            filesPagedDataTable.Rows.Add(
                                dataReader[FilesPagedDataTable.Id],
                                dataReader[FilesPagedDataTable.Name],
                                dataReader[FilesPagedDataTable.Extension],
                                dataReader[FilesPagedDataTable.MimeType]);
                        }
                    }
                }
            }

            return filesPagedDataTable;
        }
    }
}
