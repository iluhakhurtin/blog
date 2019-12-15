using System;
namespace Blog.Retrievers.PostgreSQL
{
    internal abstract class Retriever 
    {
        protected readonly string connectionString;

        public Retriever(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
