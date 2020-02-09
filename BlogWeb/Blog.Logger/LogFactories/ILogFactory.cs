using System;
using log4net;

namespace Blog.Logger.LogFactories
{
    interface ILogFactory
    {
        ILog GetLog();
    }
}
