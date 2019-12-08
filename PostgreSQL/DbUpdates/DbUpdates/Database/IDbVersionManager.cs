using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdates.Database
{
    interface IDbVersionManager
    {
        Task<int?> GetVersionAsync(string versionFieldName);
        Task SetVersionAsync(int version, string versionFieldName);
    }
}
