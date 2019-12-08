using System.Text.RegularExpressions;

namespace DbUpdates
{
    class Configuration
    {
        public string DbConnectionString { get; private set; }

        public string DbNameTemplate { get; private set; }

        public string DbServerName
        {
            get
            {
                return this.GetDbServerName();
            }
        }

        public string DbUpdatesFolderPath { get; private set; }

        public string VersionFieldName { get; private set; }

        public string RunScriptFile { get; private set; }

        public Configuration(string[] args)
        {
            //connection string and database names to update
            for (int i = 0; i < args.Length; i++)
            {
                //-cs connection string
                if(args[i] == "-cs")
                {
                    i++;
                    if (i < args.Length)
                        this.DbConnectionString = args[i];
                    else
                        return;
                }

                //-db database name template (customer, master etc)
                if(args[i] == "-db")
                {
                    i++;
                    if (i < args.Length)
                        this.DbNameTemplate = args[i];
                    else
                        return;
                }

                //-rs run script file
                if(args[i] == "-rs")
                {
                    i++;
                    if (i < args.Length)
                        this.RunScriptFile = args[i];
                    else
                        return;
                }

                //-vf version field name
                if(args[i] == "-vfn")
                {
                    i++;
                    if (i < args.Length)
                        this.VersionFieldName = args[i];
                    else
                        return;
                }

                //-uf updates folder
                if(args[i] == "-uf")
                {
                    i++;
                    if (i < args.Length)
                        this.DbUpdatesFolderPath = args[i];
                    else
                        return;
                }
            }
        }

        private string GetDbServerName()
        {
            Regex serverNameRegex = new Regex(@"(?<=Server\=).+?(?=\;)");
            Match serverNameRegexMatch = serverNameRegex.Match(this.DbConnectionString);
            return serverNameRegexMatch.Value;
        }
    }
}
