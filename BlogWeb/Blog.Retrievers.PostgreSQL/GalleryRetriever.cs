using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Retrievers.Article;
using Blog.Retrievers.Gallery;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL
{
    internal class GalleryRetriever : Retriever, IGalleryRetriever
    {
        public GalleryRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<GalleryPagedDataTable> GetGalleryPagedAsync(string smallFileNameFilter, string previewFileNameFilter,
            string originalFileNameFilter, string articleTitleFilter, string descriptionFilter, string sortColumn, string sortOrder,
            int pageNumber, int pageSize)
        {
            var galleryPagedDataTable = new GalleryPagedDataTable(pageNumber, pageSize);

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                    *
                                    FROM
                                    (
                                        SELECT
	                                        COUNT(1) OVER()		AS ""ResultsCount"",
                                            g.""Id""            AS ""Id"",
	                                        sf.""Id""           AS ""SmallFileId"",
	                                        sf.""Name""         AS ""SmallFileName"",
	                                        pf.""Id""           AS ""PreviewFileId"",
	                                        pf.""Name""         AS ""PreviewFileName"",
	                                        of.""Id""           AS ""OriginalFileId"",
	                                        of.""Name""         AS ""OriginalFileName"",
	                                        a.""Id""            AS ""ArticleId"",
	                                        a.""Title""         AS ""ArticleTitle"",
	                                        g.""Description""   AS ""Description"",
	                                        g.""Timestamp""     AS ""Timestamp""
	                                        FROM ""Gallery"" g
	                                        LEFT JOIN ""Files"" sf ON sf.""Id"" = g.""SmallPreviewFileId""
	                                        LEFT JOIN ""Images"" i ON i.""Id"" = g.""ImageId""
	                                        LEFT JOIN ""Files"" pf ON pf.""Id"" = i.""PreviewFileId""
	                                        LEFT JOIN ""Files"" of ON of.""Id"" = i.""OriginalFileId""
	                                        LEFT JOIN ""Articles"" a ON a.""Id"" = g.""ArticleId""
                                            WHERE :SmallFileNameFilter IS NULL OR sf.""Name"" ILIKE('%' || :SmallFileNameFilter || '%')
                                                AND :PreviewFileNameFilter IS NULL OR pf.""Name"" ILIKE('%' || :PreviewFileNameFilter || '%')
                                                AND :OriginalFileNameFilter IS NULL OR of.""Name"" ILIKE('%' || :OriginalFileNameFilter || '%')
                                                AND :ArticleTitleFilter IS NULL OR a.""Title"" ILIKE('%' || :ArticleTitleFilter || '%')
                                                AND :DescriptionFilter IS NULL OR g.""Description"" ILIKE('%' || :DescriptionFilter || '%')
                                    ) AS Result
                                    ORDER BY
                                        CASE
                                            WHEN :SortOrder = 'desc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'SmallFileName' THEN CAST(""SmallFileName"" AS text)
                                                    WHEN :SortColumn = 'PreviewFileName' THEN CAST(""PreviewFileName"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(""OriginalFileName"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(""ArticleTitle"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(""Description"" AS text)
                                                    WHEN :SortColumn = 'Timestamp' THEN CAST(""Timestamp"" AS text)
					                                ELSE CAST(""Id"" AS text)
				                                END
                                        END DESC,
		                                CASE
                                            WHEN :SortOrder = 'asc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'SmallFileName' THEN CAST(""SmallFileName"" AS text)
                                                    WHEN :SortColumn = 'PreviewFileName' THEN CAST(""PreviewFileName"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(""OriginalFileName"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(""ArticleTitle"" AS text)
                                                    WHEN :SortColumn = 'OriginalFileName' THEN CAST(""Description"" AS text)
                                                    WHEN :SortColumn = 'Timestamp' THEN CAST(""Timestamp"" AS text)
					                                ELSE CAST(""Id"" AS text)
				                                END
                                        END ASC
                                    LIMIT :PageSize
                                    OFFSET(:PageNumber - 1) * :PageSize;
                                    ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var filterParam = new NpgsqlParameter<string>("SmallFileNameFilter", smallFileNameFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("PreviewFileNameFilter", previewFileNameFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("OriginalFileNameFilter", originalFileNameFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("ArticleTitleFilter", articleTitleFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("DescriptionFilter", descriptionFilter);
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
                            if (galleryPagedDataTable.TotalResultsCount == 0)
                                galleryPagedDataTable.TotalResultsCount = Convert.ToInt32(dataReader["ResultsCount"]);

                            var timestamp = dataReader[GalleryPagedDataTable.Timestamp];
                            var timestampStr = timestamp == null ? String.Empty : ((DateTime)timestamp).ToShortDateString();

                            galleryPagedDataTable.Rows.Add(
                                dataReader[GalleryPagedDataTable.Id],
                                dataReader[GalleryPagedDataTable.SmallFileId],
                                dataReader[GalleryPagedDataTable.SmallFileName],
                                dataReader[GalleryPagedDataTable.PreviewFileId],
                                dataReader[GalleryPagedDataTable.PreviewFileName],
                                dataReader[GalleryPagedDataTable.OriginalFileId],
                                dataReader[GalleryPagedDataTable.OriginalFileName],
                                dataReader[GalleryPagedDataTable.ArticleId],
                                dataReader[GalleryPagedDataTable.ArticleTitle],
                                dataReader[GalleryPagedDataTable.Description],
                                timestampStr);
                        }
                    }
                }
            }

            return galleryPagedDataTable;
        }
    }
}
