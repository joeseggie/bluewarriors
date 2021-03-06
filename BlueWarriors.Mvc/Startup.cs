﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Repository;
using BlueWarriors.Mvc.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neleus.DependencyInjection.Extensions;
using NLog.Extensions.Logging;
using NLog.Web;
using BlueWarriors.Services;
using Bluewarriors.Mvc.Models;

namespace BlueWarriors.Mvc
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Secret.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            env.ConfigureNLog("nlog.config");
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<IISOptions>(options => options.ForwardWindowsAuthentication = true);
            services.AddAntiforgery();
            //call this in case you need aspnet-user-authtype/aspnet-user-identity
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAgent, Agent>();
            services.AddTransient<IAgentLeader, AgentLeader>();
            services.AddTransient<IDatabaseConnection, DatabaseConnection>();
            services.AddTransient<ISmsMessage, SmsMessage>();
            services.AddTransient<IRepository<Region>, RegionRepository<Region>>();
            services.AddTransient<IRepository<Department>, DepartmentRepository<Department>>();
            services.AddTransient<IRepository<LeaderType>, LeaderTypeRepository<LeaderType>>();
            services.AddTransient<IRepository<Leader>, LeaderRepository<Leader>>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
