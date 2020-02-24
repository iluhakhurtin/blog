using System;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Blog.Services.Tests
{
    public class StringService_Tests : BaseServiceTests
    {
        private readonly IStringService stringService;

        public StringService_Tests()
        {
            var services = base.windsorContainer.Resolve<IServices>();
            this.stringService = services.StringService;
        }

        [Fact]
        public void Can_ParseCsvStringToArray()
        {
            var csvString = "string1,string2, string3, string.4, ";

            var actual = this.stringService.ParseCsvStringToArray(csvString);

            Assert.Equal(4, actual.Length);
            Assert.Equal("string1", actual[0]);
            Assert.Equal("string2", actual[1]);
            Assert.Equal("string3", actual[2]);
            Assert.Equal("string.4", actual[3]);
        }
    }
}
