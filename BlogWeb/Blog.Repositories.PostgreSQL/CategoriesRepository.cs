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
                        if (await dataReader.ReadAsync())
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
    }
}
