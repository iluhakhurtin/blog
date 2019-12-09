using System;
using System.Collections.Generic;
using System.Text;

namespace SanaLive.Service.DbConnections
{
    public interface IDbConnectionInfo
    {
        string DbServer { get; }
        string DbConnectionString { get; }
    }

    public class DbConnectionInfo : IDbConnectionInfo
    {
        public string DbServer { get; set; }

        public string DbConnectionString { get; set; }

        public DbConnectionInfo()
        {
        }

        public DbConnectionInfo(string dbServerName, string dbConnectionString)
        {
            this.DbServer = dbServerName;
            this.DbConnectionString = dbConnectionString;
        }
    }
}
