using DbUpdates.Database;
using DbUpdates.PostgreSQL;
using DbUpdates.UpdateScripts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DbUpdates
{
    class Program
    {
        private static IDbManager dbManager;
        private static Configuration configuration;

        static void Main(string[] args)
        {
            try
            {
                configuration = new Configuration(args);
                dbManager = new DbManager(configuration.DbConnectionString);

                PrintConfigurationInformation();

                bool ifContinue = DoYouWantToContinue();
                if (ifContinue)
                {
                    UpdateDatabases();
                }
                else
                {
                    UpdateCancelled();
                }
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static bool DoYouWantToContinue()
        {
            Console.WriteLine("Do you want to continue (y/n)?");
            ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
            return consoleKeyInfo.KeyChar == 'y';
        }

        private static void PrintConfigurationInformation()
        {
            Console.WriteLine("This program will update all databases with name like: ");
            Console.WriteLine();
            Console.WriteLine(configuration.DbNameTemplate);
            Console.WriteLine();
            Console.WriteLine("on the server: ");
            Console.WriteLine();
            Console.WriteLine(configuration.DbServerName);
            Console.WriteLine();
        }

        private static void UpdateCancelled()
        {
            Console.WriteLine();
            Console.WriteLine("Update cancelled");
        }

        private static void UpdateDatabases()
        {
            Console.WriteLine();

            var updatingDbsList = dbManager.GetDatabasesAsync(configuration.DbNameTemplate).Result;
            if(updatingDbsList.Count == 0)
            {
                NothingToUpdate();
                return;
            }

            Console.WriteLine("The databases to be updated: ");
            foreach (string dbName in updatingDbsList)
            {
                Console.WriteLine(dbName);
            }
            Console.WriteLine();

            bool ifContinue = DoYouWantToContinue();
            if(ifContinue)
            {
                string runScriptFileContent = GetRunScriptFileContent(configuration.RunScriptFile);
                RunDbUpdates(
                    updatingDbsList, 
                    runScriptFileContent, 
                    configuration.VersionFieldName,
                    configuration.DbUpdatesFolderPath,
                    configuration.DbConnectionString);
            }
            else
            {
                UpdateCancelled();
            }
        }

        private static void NothingToUpdate()
        {
            Console.WriteLine("There is not a database matching the db name template ({0}) on the sever {1}. Check appsettings.json config.", configuration.DbNameTemplate, configuration.DbServerName);
            Console.WriteLine();
        }

        private static string GetRunScriptFileContent(string runScriptFile)
        {
            if (String.IsNullOrEmpty(runScriptFile))
                return null;

            if (!File.Exists(runScriptFile))
                throw new ArgumentException("Script file to run does not exist.");

            string runScriptFileContent = File.ReadAllText(runScriptFile);

            return runScriptFileContent;
        }

        private static void RunDbUpdates(
            IList<string> dbNames, 
            string runScriptFileContent, 
            string versionFieldName, 
            string updatesFolderPath, 
            string connectionString)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("... Updating ...");
            Parallel.ForEach<string>(dbNames, dbName =>
            {
                if (String.IsNullOrEmpty(runScriptFileContent))
                {
                    RunDbUpdate(dbName, versionFieldName, updatesFolderPath, connectionString);
                }
                else
                {
                    RunDbScriptUpdate(dbName, runScriptFileContent);
                }
            });
            Console.WriteLine();
        }

        private static bool RunDbUpdate(
            string dbName, 
            string versionFieldName, 
            string updatesFolderPath, 
            string connectionString)
        {
            IDbVersionManager dbVersionManager = new DbVersionManager(dbName, connectionString);

            int? dbVersion = dbVersionManager.GetVersionAsync(versionFieldName).Result;
            if (dbVersion.HasValue)
                dbVersion++;
            else
                dbVersion = 0;

            IUpdateScriptManager updateScriptManager = new UpdateScriptManager(updatesFolderPath);
            IDbUpdateRunner dbUpdateRunner = new DbUpdateRunner(dbName, connectionString);

            bool isUpdated = false;

            foreach(string updateContent in updateScriptManager.GetUpdatesContent(dbVersion.Value))
            {
                isUpdated = true;

                dbUpdateRunner.RunUpdateAsync(updateContent).Wait();
                dbVersionManager.SetVersionAsync(dbVersion.Value, versionFieldName).Wait();

                Console.WriteLine("{0} -> {1} updated to version {2}", dbName, versionFieldName, dbVersion);

                dbVersion++;
            }

            if (!isUpdated)
            {
                Console.WriteLine("Database {0} has not been updated because it is up to date.", dbName);
            }

            return isUpdated;
        }

        private static void RunDbScriptUpdate(string dbName, string runScriptFileContent)
        {
            IDbUpdateRunner dbUpdateRunner = new DbUpdateRunner(dbName, configuration.DbConnectionString);
            dbUpdateRunner.RunUpdateAsync(runScriptFileContent).Wait();

            Console.WriteLine("{0} updated", dbName);
        }
    }
}
