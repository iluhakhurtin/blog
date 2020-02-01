using Microsoft.Extensions.Configuration;
using System.IO;

namespace Blog.Retrievers.PostgreSQL.Tests
{
    public static class ConnectionStrings
    {
        private static IConfiguration configuration;

        static ConnectionStrings()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            configurationBuilder.AddJsonFile("appsettings.Development.json");
#else
            configurationBuilder.AddJsonFile("appsettings.json");
#endif
            configuration = configurationBuilder.Build();
        }

        public static string BlogPostgreSQLConnection
        {
            get
            {
                return configuration.GetConnectionString("BlogPostgreSQLConnection");
            }
        }
    }
}