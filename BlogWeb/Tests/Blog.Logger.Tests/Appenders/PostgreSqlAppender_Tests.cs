using System;
using System.IO;
using System.Threading.Tasks;
using Blog.Logger.Appenders;
using Blog.PostgreSQL.Tests;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Blog.Logger.Tests.Appenders
{
    public class PostgreSqlAppender_Tests : IDisposable
    {
        string connectionString;
        ILog logger;
        string repositoryName;

        public PostgreSqlAppender_Tests()
        {
            this.connectionString = this.GetConnectionString();
            this.CleanDatabase(this.connectionString).Wait();
            this.logger = this.InitializeLogger(this.connectionString);
        }

        private string GetConnectionString()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            configurationBuilder.AddJsonFile("appsettings.Development.json");
#else
            configurationBuilder.AddJsonFile("appsettings.json");
#endif
            IConfigurationRoot configuration = configurationBuilder.Build();
            string connectionString = configuration.GetConnectionString("BlogPostgreSQLConnection");
            return connectionString;
        }

        private ILog InitializeLogger(string connectionString)
        {
            var filter = new LevelMatchFilter();
            filter.LevelToMatch = Level.All;
            filter.ActivateOptions();

            var appender = new PostgreSqlAppender(connectionString);
            appender.AddFilter(filter);
            appender.ActivateOptions();

            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().FullName;
            this.repositoryName = String.Format("{0} Repository", assemblyName);

            var repository = LoggerManager.CreateRepository(repositoryName);

            string loggerName = String.Format("{0} Logger", assemblyName);
            BasicConfigurator.Configure(repository, appender);
            ILog logger = LogManager.GetLogger(repositoryName, loggerName);
            return logger;
        }

        public void Dispose()
        {
            this.CleanLogger();
            this.CleanDatabase(this.connectionString).Wait();
        }

        private void CleanLogger()
        {
            this.logger.Logger.Repository.Shutdown();
        }

        private async Task CleanDatabase(string connectionString)
        {
            await DatabaseCleaner.CleanUp(connectionString);
        }


        [Fact]
        public void Can_Log()
        {
            this.logger.Debug("test debug message");
            this.logger.Error("test debug message");
            this.logger.Fatal("test fatal message");
            this.logger.Info("test info message");
            this.logger.Warn("test warn message");
        }
    }
}
