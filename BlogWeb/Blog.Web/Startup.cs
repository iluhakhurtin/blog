using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SanaLive.Service.DbConnections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog.Domain;

namespace Blog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllersWithViews();

            IDbConnections dbConnections = new JsonDbConnections();
            serviceCollection.AddSingleton<IDbConnections>(dbConnections);

            this.InitIdentity(serviceCollection, dbConnections.BlogConnectionString);

            IRepositories repositories = this.BuildRepositories(dbConnections.BlogConnectionString);
            serviceCollection.AddSingleton<IRepositories>(repositories);

            IRetrievers retrievers = this.BuildRetrievers(dbConnections.BlogConnectionString);
            serviceCollection.AddSingleton<IRetrievers>(retrievers);

            IServices services = this.BuildServices(repositories, retrievers);
            serviceCollection.AddSingleton<IServices>(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "article",
                    pattern: "Article/Index/images/{fileName}",
                    new { controller = "Article", action = "Image", arg = "{fileName}" });

                var imageRoutingConstraint = new RegexRoutingConstraint("/(?<images>)/.+\\..+?$");

                endpoints.MapControllerRoute(
                    name: "images",
                    pattern: "{*url}",
                    defaults: new { controller = "Image", action = "Image" },
                    constraints: new { url = imageRoutingConstraint });
            });

            this.InitAdminUserAndRoles(serviceProvider).Wait();
        }

        private IRepositories BuildRepositories(string blogConnectionString)
        {
            IRepositories repositories = new Blog.Repositories.PostgreSQL.Repositories(blogConnectionString);
            return repositories;
        }

        private IRetrievers BuildRetrievers(string blogConnectionString)
        {
            IRetrievers retrievers = new Blog.Retrievers.PostgreSQL.Retrievers(blogConnectionString);
            return retrievers;
        }

        private IServices BuildServices(IRepositories repositories, IRetrievers retrievers)
        {
            IServices services = new Blog.Services.Services(repositories, retrievers);
            return services;
        }

        private void InitIdentity(IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<IdentityDbContext>(options => options.UseNpgsql(connectionString));

            serviceCollection.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            serviceCollection.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // Lockout settings  
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings  
                options.User.RequireUniqueEmail = true;
            });

            //Seting the Account Login page  
            serviceCollection.ConfigureApplicationCookie(options =>
            {
                // Cookie settings  
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login  
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout  
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied  
                options.SlidingExpiration = true;
            });
        }

        private async Task InitAdminUserAndRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleResult;
            //Adding Admin Role 
            var roleCheck = await roleManager.RoleExistsAsync(Roles.Administrator);
            if (!roleCheck)
            {
                //create the roles and seed them to the database  
                roleResult = await roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            }

            //Adding Private Reader role
            roleCheck = await roleManager.RoleExistsAsync(Roles.PrivateReader);
            if (!roleCheck)
            {
                //create the role if it does not exists
                roleResult = await roleManager.CreateAsync(new IdentityRole(Roles.PrivateReader));
            }

            //Assign Admin role to the main User

            string adminEmal = "admin@blog.com";
            IdentityUser user = await userManager.FindByEmailAsync(adminEmal);

            if (user == null)
            {
                user = new IdentityUser();
                user.Email = adminEmal;
                user.UserName = adminEmal;
                await userManager.CreateAsync(user, "lkjaksdf");
            }

            await userManager.AddToRoleAsync(user, Roles.Administrator);
        }
    }

    public class RegexRoutingConstraint : IRouteConstraint
    {
        private readonly List<Regex> regexes = new List<Regex>();

        public RegexRoutingConstraint(params string[] regexes)
        {
            foreach (var regex in regexes)
            {
                this.regexes.Add(new Regex(regex));
            }
        }

        public bool Match(HttpContext httpContext,
                          IRouter route,
                          string routeKey,
                          RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (values[routeKey] == null)
            {
                return false;
            }

            var url = values[routeKey].ToString();

            foreach (var regex in regexes)
            {
                var match = regex.Match(url);
                if (match.Success)
                {
                    foreach (Group group in match.Groups)
                    {
                        values.Add(group.Name, group.Value);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
