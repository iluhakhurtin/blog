using System;
using System.Threading.Tasks;
using Blog.PostgreSQL.Tests;
using Xunit;

namespace Blog.Retrievers.PostgreSQL.Tests
{
    [Collection("Blog")]
    public abstract class BaseRetrieverTests : IDisposable
    {
        public BaseRetrieverTests()
        {
            this.CleanDatabase().Wait();
        }

        public void Dispose()
        {
            this.CleanDatabase().Wait();
        }

        private async Task CleanDatabase()
        {
            await DatabaseCleaner.CleanUp(ConnectionStrings.BlogPostgreSQLConnection);
        }
    }
}
