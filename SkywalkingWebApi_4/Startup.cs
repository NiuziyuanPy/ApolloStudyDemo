using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SkyApm.Sample.Backend.Models;
using SkyApm.Sample.Backend.Sampling;
using SkyApm.Tracing;

namespace SkywalkingWebApi_4
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var sqlLiteConnection = new MySqlConnection("Data Source=192.168.2.96;User Id=root;password=123qweasd,./;Database=UserInfo_Test");
            sqlLiteConnection.Open();

            services.AddEntityFrameworkSqlite().AddDbContext<SampleDbContext>(c => c.UseSqlite(sqlLiteConnection));

            services.AddSingleton<ISamplingInterceptor, CustomSamplingInterceptor>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    using var sampleDbContext = scope.ServiceProvider.GetService<SampleDbContext>();
            //    sampleDbContext.Database.EnsureCreated();
            //}

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
