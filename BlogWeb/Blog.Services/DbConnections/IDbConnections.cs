using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SanaLive.Service.DbConnections
{
    public interface IDbConnections
    {
        string BlogConnectionString { get; }
    }
}
