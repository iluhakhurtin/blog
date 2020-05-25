using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Repository;
using Microsoft.Extensions.Configuration;

namespace Blog.Logger.LogFactories
{
    public class JsonLogFactory : ILogFactory
    {
        private string fileName;
        private string fileFolderPath;
        private IConfigurationRoot configurationRoot;

        public JsonLogFactory()
        {
            this.fileFolderPath = Directory.GetCurrentDirectory();
            this.fileName = "appsettings.json";
        }

        public JsonLogFactory(string configFileFolderPath, string configFileName)
        {
            this.fileFolderPath = configFileFolderPath;
            this.fileName = configFileName;
        }

        public JsonLogFactory(IConfigurationRoot configurationRoot)
        {
            this.configurationRoot = configurationRoot;
        }

        public ILog GetLog()
        {
            if(this.configurationRoot == null)
                this.configurationRoot = this.GetConfigurationRoot(this.fileFolderPath, this.fileName);

            IConfigurationSection log4netSection = this.GetLog4NetConfigurationSection(this.configurationRoot);
            IEnumerable<AppenderSkeleton> appenders = this.GetAppenders(this.configurationRoot, log4netSection);
            IConfigurationSection loggerSection = this.GetLoggerSection(log4netSection);

            IConfigurationSection name = loggerSection.GetSection("name");
            string repositoryName = String.Format("{0} Repository", name.Value);
            ILoggerRepository repository = LoggerManager.CreateRepository(repositoryName);
            string loggerName = String.Format("{0} Logger", name.Value);

            IConfigurationSection level = loggerSection.GetSection("level");
            string levelName = "All";
            if (level != null && !String.IsNullOrEmpty(level.Value))
                levelName = level.Value;
            ILoggerRepository loggerRepository = LoggerManager.GetAllRepositories().FirstOrDefault();
            LevelMatchFilter filter = new LevelMatchFilter();
            filter.LevelToMatch = loggerRepository.LevelMap[levelName];
            filter.ActivateOptions();

            foreach (var appender in appenders)
            {
                appender.AddFilter(filter);
                appender.ActivateOptions();

                BasicConfigurator.Configure(repository, appender);
            }

            ILog logger = LogManager.GetLogger(repositoryName, loggerName);
            return logger;
        }

        private IConfigurationRoot GetConfigurationRoot(string configFileFolderPath, string configFileName)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(configFileFolderPath);
            configurationBuilder.AddJsonFile(configFileName);
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            return configurationRoot;
        }

        private IConfigurationSection GetLog4NetConfigurationSection(IConfigurationRoot configurationRoot)
        {
            IConfigurationSection log4netSection = configurationRoot.GetSection("log4net");
            return log4netSection;
        }

        private IEnumerable<AppenderSkeleton> GetAppenders(IConfigurationRoot configurationRoot, IConfigurationSection log4netSection)
        {
            IEnumerable<IConfigurationSection> children = log4netSection.GetChildren();
            foreach (var child in children)
            {
                if (child.Key == "appender")
                {
                    IConfigurationSection name = child.GetSection("name");
                    IConfigurationSection type = child.GetSection("type");
                    Type appenderType = Type.GetType(type.Value);
                    ConstructorInfo[] constructorInfos = appenderType.GetConstructors();
                    ConstructorInfo constructorInfo = constructorInfos[0];
                    List<object> arguments = new List<object>();
                    foreach (ParameterInfo param in constructorInfo.GetParameters())
                    {
                        object argument = null;
                        if (param.Name == "connectionString")
                        {
                            //get connection string from connection strings section
                            IConfigurationSection connectionStringName = child.GetSection("connectionStringName");
                            string connectionString = configurationRoot.GetConnectionString(connectionStringName.Value);
                            argument = connectionString;
                        }
                        else
                        {
                            IConfigurationSection paramConfigurationSection = child.GetSection(param.Name);
                            argument = Convert.ChangeType(paramConfigurationSection.Value, param.ParameterType);
                        }
                        arguments.Add(argument);
                    }
                    AppenderSkeleton appender = Activator.CreateInstance(appenderType, arguments.ToArray()) as AppenderSkeleton;
                    yield return appender;
                }
            }
        }

        private IConfigurationSection GetLoggerSection(IConfigurationSection log4netSection)
        {
            IConfigurationSection loggerSection = log4netSection.GetSection("logger");
            return loggerSection;
        }
    }
}
