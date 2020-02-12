using System;
using Blog.Logger.LogFactories;
using log4net;
using Xunit;

namespace Blog.Logger.Tests.LogFactories
{
    public class JsonLogFactory_Tests
    {
        [Fact]
        public void Can_ReadLog4NetConfig()
        {
            var logFactory = new JsonLogFactory();
            var log = logFactory.GetLog();

            Assert.Equal("Blog.Logger.Tests Logger", log.Logger.Name);
            Assert.Equal("Blog.Logger.Tests Repository", log.Logger.Repository.Name);

            log.Logger.Repository.Shutdown();
        }
    }
}
