using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdates.Database
{
    interface IDbManager
    {
        Task<IList<string>> GetDatabasesAsync(string dbNameTemplate);
    }
}
