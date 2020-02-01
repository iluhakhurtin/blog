using System;
using Blog.Retrievers.Article;
using Xunit;

namespace Blog.Retrievers.Tests.Article
{
    public class ArticleWithRolesDataResult_Tests
    {
        [Fact]
        public void Can_GetRoles()
        {
            var articleWithRolesDataResult = new ArticleWithRolesDataResult();
            articleWithRolesDataResult.Roles = "Role1, Role2";

            var actual = articleWithRolesDataResult.GetRoles();

            Assert.Equal(2, actual.Length);
            Assert.Equal("Role1", actual[0]);
            Assert.Equal("Role2", actual[1]);
        }
    }
}
