
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Blog.Retrievers;
using Blog.Repositories;
using Microsoft.EntityFrameworkCore;
using Blog.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;

namespace Blog.Retrievers.PostgreSQL.Tests
{
    public static class DependencyResolver
    {
        private static IWindsorContainer windsorContainer;
        private static ApplicationDbContext dbContext;

        static DependencyResolver()
        {
            Initialize(ConnectionStrings.BlogPostgreSQLConnection);
        }

        static void Initialize(string blogConnectionString)
        {
            windsorContainer = new WindsorContainer();
            RegisterComponents(windsorContainer, blogConnectionString);
        }

        private static void RegisterComponents(IWindsorContainer container, string blogConnectionString)
        {
            RegisterRetrievers(container, blogConnectionString);
            RegisterRepositories(container, blogConnectionString);
            RegisterUserManager(container, blogConnectionString);
            RegisterRoleManager(container, blogConnectionString);
        }

        private static void RegisterRepositories(IWindsorContainer container, string blogConnectionString)
        {
            container.Register(
                Component.For<IRepositories>()
                .ImplementedBy<Blog.Repositories.PostgreSQL.Repositories>()
                .DependsOn(Dependency.OnValue("blogConnectionString", blogConnectionString))
            );
        }

        private static void RegisterRetrievers(IWindsorContainer container, string blogConnectionString)
        {            
            container.Register(
                Component.For<IRetrievers>()
                .ImplementedBy<Blog.Retrievers.PostgreSQL.Retrievers>()
                .DependsOn(Dependency.OnValue("blogConnectionString", blogConnectionString))
            );
        }

        private static void RegisterUserManager(IWindsorContainer container, string connectionString)
        {
            var dbContext = GetDbContext(connectionString);
            IUserStore<ApplicationUser> userStore = new UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>(dbContext);
            IPasswordHasher<ApplicationUser> userHasher = new PasswordHasher<ApplicationUser>();
            UserValidator<ApplicationUser> userValidator = new UserValidator<ApplicationUser>();
            var validators = new UserValidator<ApplicationUser>[] { userValidator };

            var userManager = new UserManager<ApplicationUser>(userStore, null, userHasher, validators, null, null, null, null, null);

            // Set-up token providers.
            IUserTwoFactorTokenProvider<ApplicationUser> tokenProvider = new EmailTokenProvider<ApplicationUser>();
            userManager.RegisterTokenProvider("Default", tokenProvider);
            IUserTwoFactorTokenProvider<ApplicationUser> phoneTokenProvider = new PhoneNumberTokenProvider<ApplicationUser>();
            userManager.RegisterTokenProvider("PhoneTokenProvider", phoneTokenProvider);

            container.Register(Component.For<UserManager<ApplicationUser>>().Instance(userManager));
        }

        private static void RegisterRoleManager(IWindsorContainer container, string connectionString)
        {
            var dbContext = GetDbContext(connectionString);
            IRoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole, ApplicationDbContext, Guid>(dbContext);
            var roleValidator = new RoleValidator<ApplicationRole>();
            var roleValidators = new IRoleValidator<ApplicationRole>[] { roleValidator };

            var roleManager = new RoleManager<ApplicationRole>(roleStore, roleValidators, null, null, null);

            container.Register(Component.For<RoleManager<ApplicationRole>>().Instance(roleManager));
        }

        private static ApplicationDbContext GetDbContext(string connectionString)
        {
            if (dbContext == null)
            {
                DbContextOptionsBuilder<ApplicationDbContext> contextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                contextOptionsBuilder.UseNpgsql(connectionString);
                dbContext = new ApplicationDbContext(contextOptionsBuilder.Options);
            }
            return dbContext;
        }

        public static T Resolve<T>()
        {
            return windsorContainer.Resolve<T>();
        }
    }
}
