using System;
using System.IO;
using System.Threading.Tasks;
using Blog.Retrievers.Image;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL
{
    internal class ImagesRetriever : Retriever, IImagesRetriever
    {
        public ImagesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<ImageDataResult> GetOriginalImageDataAsync(Guid imageId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                f.""MimeType"",
	                            f.""Data""
                                FROM ""Images"" i
                                JOIN ""Files"" f ON f.""Id"" = i.""OriginalFileId""
                                WHERE i.""Id"" = :ImageId;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("ImageId", imageId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<ImageDataResult> GetPreviewImageDataAsync(Guid imageId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                f.""MimeType"",
	                            f.""Data""
                                FROM ""Images"" i
                                JOIN ""Files"" f ON f.""Id"" = i.""PreviewFileId""
                                WHERE i.""Id"" = :ImageId;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var imageIdParam = command.Parameters.AddWithValue("ImageId", imageId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<ImageDataResult> GetPreviewImageDataByNameAsync(string fileName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                            prevf.""MimeType"",
                                prevf.""Data""
                                FROM ""Files"" f
                                JOIN ""Images"" i ON i.""OriginalFileId"" = f.""Id"" OR i.""PreviewFileId"" = f.""Id""
                                JOIN ""Files"" prevf ON prevf.""Id"" = i.""PreviewFileId""
                                WHERE f.""Name"" = :FileName
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var fileNameParam = command.Parameters.AddWithValue("FileName", fileName);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<ImageDataResult> GetOriginalImageDataByNameAsync(string fileName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                            prevf.""MimeType"",
                                prevf.""Data""
                                FROM ""Files"" f
                                JOIN ""Images"" i ON i.""OriginalFileId"" = f.""Id"" OR i.""PreviewFileId"" = f.""Id""
                                JOIN ""Files"" prevf ON prevf.""Id"" = i.""OriginalFileId""
                                WHERE f.""Name"" = :FileName
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var fileNameParam = command.Parameters.AddWithValue("FileName", fileName);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<ImagesPagedDataTable> GetImagesPagedAsync(
            string imageIdFilter,
            string previewFileNameFilter,
            string originalFileNameFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize)
        {
            var imagesPagedDataTable = new ImagesPagedDataTable(pageNumber, pageSize);

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                    *
                                    FROM
		                            (SELECT
                                        COUNT(1) OVER()         AS ""ResultsCount"",
                                        i.""Id""                AS ""Id"",
                                        i.""PreviewFileId""     AS ""PreviewFileId"",
                                        pf.""Name""             AS ""PreviewFileName"",
                                        i.""OriginalFileId""    AS ""OriginalFileId"",
                                        of.""Name""             AS ""OriginalFileName""
                                        FROM ""Images"" i
                                        LEFT JOIN ""Files"" pf ON pf.""Id"" = i.""PreviewFileId""
                                        LEFT JOIN ""Files"" of ON of.""Id"" = i.""OriginalFileId""
                                    ) AS Subquery
                                    WHERE   :ImageIdFilter IS NULL OR Subquery.""Id"" = :ImageIdFilter
                                            AND :PreviewFileNameFilter IS NULL OR Subquery.""PreviewFileName"" ILIKE('%' || :PreviewFileNameFilter || '%')
                                            AND :OriginalFileNameFilter IS NULL OR Subquery.""OriginalFileName"" ILIKE('%' || :OriginalFileNameFilter || '%')
                                    ORDER BY
                                        CASE
                                            WHEN :SortOrder = 'desc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'PreviewFileName' THEN CAST(Subquery.""PreviewFileName"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(Subquery.""OriginalFileName"" AS text)
					                                ELSE CAST(Subquery.""Id"" AS text)
				                                END
                                        END DESC,
		                                CASE
                                            WHEN :SortOrder = 'asc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'PreviewFileName' THEN CAST(Subquery.""PreviewFileName"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(Subquery.""OriginalFileName"" AS text)
					                                ELSE CAST(Subquery.""Id"" AS text)
				                                END
                                        END ASC
                                    LIMIT :PageSize
                                    OFFSET(:PageNumber - 1) * :PageSize;
                                    ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var imageIdFilterParam = new NpgsqlParameter();
                    imageIdFilterParam.ParameterName = "ImageIdFilter";
                    imageIdFilterParam.DbType = System.Data.DbType.Guid;
                    if (!String.IsNullOrEmpty(imageIdFilter))
                        imageIdFilterParam.Value = Guid.Parse(imageIdFilter);
                    else
                        imageIdFilterParam.Value = DBNull.Value;
                    command.Parameters.Add(imageIdFilterParam);

                    var filterParam = new NpgsqlParameter<string>("PreviewFileNameFilter", previewFileNameFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("OriginalFileNameFilter", originalFileNameFilter);
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
                            if (imagesPagedDataTable.TotalResultsCount == 0)
                                imagesPagedDataTable.TotalResultsCount = Convert.ToInt32(dataReader["ResultsCount"]);

                            imagesPagedDataTable.Rows.Add(
                                dataReader[ImagesPagedDataTable.Id],
                                dataReader[ImagesPagedDataTable.PreviewFileId],
                                dataReader[ImagesPagedDataTable.PreviewFileName],
                                dataReader[ImagesPagedDataTable.OriginalFileId],
                                dataReader[ImagesPagedDataTable.OriginalFileName]);
                        }
                    }
                }
            }

            return imagesPagedDataTable;
        }
    }
}
