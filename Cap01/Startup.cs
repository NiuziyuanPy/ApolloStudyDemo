using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Cap01
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
            services.AddDbContext<AppDbContext>();

            services.AddCap(x =>
            {
                x.UseEntityFramework<AppDbContext>(); //数据库信息
                x.UseRabbitMQ(mq =>
                {
                    mq.HostName = "10.0.4.52";
                    mq.Password = "guest";
                    mq.UserName = "guest";
                    mq.VirtualHost = "/";
                });//MQ信息
                x.UseDashboard();//仪表信息
       
                x.FailedRetryCount = 15;// 失败重试最高次，默认是50次
                
                x.FailedRetryInterval = 30;// 失败重试间隔，默认是60s

                // 超出最高重试次数的回调
                x.FailedThresholdCallback = failedInfo =>
                {
                    // 这里通过发邮件、短信等信息通知人工处理，系统不能自动搞定了
                    Console.WriteLine("需要人工处理啦");
                };

                // 设置发送消息的线程数据，大于1之后，不保证消息顺序
                //x.ProducerThreadCount = 1;
                // 成功消息保存的时间，以秒为单位，默认是1天
                x.SucceedMessageExpiredAfter = 12 * 3600;

            });

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "OrderApi",
                    Version = "v1",
                    Description = "订单服务",
                    //TermsOfService = "None",
                    Contact = new OpenApiContact
                    {
                        Name = "Mr.Niu",
                        Email = string.Empty,
                        //Url =new Uri("www.tect.com")
                    }
                });
                //c.OperationFilter<AuthHeaderParameter>();

                c.DocInclusionPredicate((docName, description) => true);
                c.CustomSchemaIds(type => type.FullName);

                var xmlFile = "Cap01.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderApi V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
