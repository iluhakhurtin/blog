using System;
using System.Linq;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal abstract class Repository<T> : IRepository<T>
    {
        protected readonly string connectionString;

        public Repository(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
