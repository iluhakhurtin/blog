using System;
using System.Collections.Generic;
using System.Text;

namespace DbUpdates.UpdateScripts
{
    interface IUpdateScriptManager
    {
        IEnumerable<string> GetUpdatesContent(int version);
    }
}
