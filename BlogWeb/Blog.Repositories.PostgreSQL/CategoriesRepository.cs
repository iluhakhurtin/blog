using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class CategoriesRepository : Repository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<IList<Category>> GetAllAsync()
        {
            var categories = new List<Category>();
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                ""Id"",
                                ""Name"",
                                ""ParentId""
                                FROM ""Categories"";
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var category = new Category();
                            category.Id = (Guid)dataReader["Id"];
                            category.Name = Convert.ToString(dataReader["Name"]);
                            category.ParentId = dataReader["ParentId"] == DBNull.Value ? null : (Guid?)dataReader["ParentId"];

                            categories.Add(category);
                        }
                    }
                }
            }
            return categories;
        }

        public async Task AddAsync(Category category)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    INSERT INTO ""Categories""(
                        ""Id"",
	                    ""Name"", 
	                    ""ParentId""
	                )
	                VALUES (
                        :Id,
	                    :Name, 
	                    :ParentId
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", category.Id);
                    command.Parameters.AddWithValue(":Name", category.Name);

                    var parentIdParam = new NpgsqlParameter(":ParentId", (object)category.ParentId ?? DBNull.Value);
                    command.Parameters.Add(parentIdParam);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
    }
}
