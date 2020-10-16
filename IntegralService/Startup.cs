using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegralService.MiddleWare;
using IntegralService.Scheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;

namespace IntegralService
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
            string connectionString = Configuration.GetSection("ConnectionString")["Connection"];
            services.AddDbContext<IntegralContext>(options =>
            {
                options.UseSqlServer(connectionString, m =>
                        m.MigrationsAssembly("IntegralService"));
            }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            //app.UseIntegralMiddleWare();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
