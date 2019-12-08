using System;
using System.Collections.Generic;
using System.Text;

namespace DbUpdates.PostgreSQL
{
    abstract class DbBase
    {
        protected string ConnectionString { get; private set; }

        public DbBase(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
