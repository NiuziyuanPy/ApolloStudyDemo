using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Shared;
using Tect.Shared;

namespace Consul01
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


            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    //TermsOfService = "None",
                    Contact = new OpenApiContact
                    {
                        Name = "Mr.Wang",
                        Email = string.Empty,
                        //Url =new Uri("www.tect.com")
                    }
                });
                //c.OperationFilter<AuthHeaderParameter>();

                c.DocInclusionPredicate((docName, description) => true);
                c.CustomSchemaIds(type => type.FullName);

                var xmlFile = "Consul01.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });



            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            var ip = Environment.GetEnvironmentVariable("HOST_IP");

            var port = Environment.GetEnvironmentVariable("HOST_PORT");

            var serverName = Environment.GetEnvironmentVariable("SERVER_NAME");
            //api/Consul01/HealthCheck
            if (!string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(port))
            {
                ServiceEntity serviceEntity = new ServiceEntity
                {
                    IP = ip,
                    Port = Convert.ToInt32(port),
                    ServiceName = serverName,
                    ServerPath = "http://10.0.4.52:8500",
                    CheckPath = "/api/Consul01/HealthCheck",
                    //Token = configuration["Consul:Token"]
                };
                app.RegisterConsul(lifetime, serviceEntity);
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
