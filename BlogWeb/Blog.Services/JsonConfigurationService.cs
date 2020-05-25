using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Blog.Services
{
    public interface IConfigurationService
    {
        string BlogConnectionString { get; }
        IConfigurationRoot ConfigurationRoot { get; }
    }

    public class ConfigurationService : Service, IConfigurationService
    {
        public string BlogConnectionString { get; private set; }

        public IConfigurationRoot ConfigurationRoot { get; private set; }

        public ConfigurationService()
        {
            this.ReadConfiguration();
        }

        private void ReadConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());

#if DEBUG
            configurationBuilder.AddJsonFile("appsettings.Development.json");
#else
            configurationBuilder.AddJsonFile("appsettings.json");
#endif

            this.ConfigurationRoot = configurationBuilder.Build();
            this.BlogConnectionString = this.ConfigurationRoot.GetConnectionString("BlogPostgreSQLConnection");
        }
    }
}
