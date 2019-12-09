using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SanaLive.Service.DbConnections
{
    public class JsonDbConnections : IDbConnections
    {
        public string BlogConnectionString { get; private set; }
        
        public JsonDbConnections()
        {
            this.InitializeDbConnections();
        }

        private void InitializeDbConnections()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());

#if DEBUG
            configurationBuilder.AddJsonFile("appsettings.Development.json");
#else
            configurationBuilder.AddJsonFile("appsettings.json");
#endif

            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            this.BlogConnectionString = configurationRoot.GetConnectionString("BlogPostgreSQLConnection");
        }
    }
}
