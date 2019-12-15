using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repositories;
using Blog.Retrievers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SanaLive.Service.DbConnections;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            IDbConnections dbConnections = new JsonDbConnections();
            services.AddSingleton<IDbConnections>(dbConnections);

            //repositories
            IRepositories repositories = this.BuildRepositories(dbConnections.BlogConnectionString);
            services.AddSingleton<IRepositories>(repositories);

            IRetrievers retrievers = this.BuildRetrievers(dbConnections.BlogConnectionString);
            services.AddSingleton<IRetrievers>(retrievers);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            });
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
    }
}
