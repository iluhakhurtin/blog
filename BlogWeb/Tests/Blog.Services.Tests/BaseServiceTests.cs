using System;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Blog.Services.Tests
{
    [Collection("Blog")]
    public abstract class BaseServiceTests
    {
        protected IWindsorContainer windsorContainer;

        public BaseServiceTests()
        {
            var repositories = this.InitRepositories();
            var retrievers = this.InitRetrievers();
            var roleManager = this.InitRoleManager();

            this.windsorContainer = InitWindsorContainer(repositories, retrievers, roleManager);
        }

        protected virtual RoleManager<ApplicationRole> InitRoleManager()
        {
            var mockRoleStore = new Mock<IRoleStore<ApplicationRole>>();
            var mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
                mockRoleStore.Object, null, null, null, null);
            return mockRoleManager.Object;
        }

        protected virtual IRetrievers InitRetrievers()
        {
            var mockRetrievers = new Mock<IRetrievers>();
            return mockRetrievers.Object;
        }

        protected virtual IRepositories InitRepositories()
        {
            var mockRepositories = new Mock<IRepositories>();
            return mockRepositories.Object;
        }

        protected IWindsorContainer InitWindsorContainer(
            IRepositories repositories,
            IRetrievers retrievers,
            RoleManager<ApplicationRole> roleManager)
        {
            var windsorContainer = new WindsorContainer();
            windsorContainer.Register(
                Component.For<IServices>()
                .ImplementedBy<Blog.Services.Services>()
                .DependsOn(Dependency.OnValue("repositories", repositories))
                .DependsOn(Dependency.OnValue("retrievers", retrievers))
                .DependsOn(Dependency.OnValue("roleManager", roleManager))
            );

            return windsorContainer;
        }
    }
}
