using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Tect.Shared;

namespace Shared
{
    public static class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            var ip = configuration["ip"];
            var port = int.Parse(configuration["port"]);
            var weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);
            var name = configuration["Consul:GroupName"];
            var checkPath = configuration["Consul:CheckPath"];
            var serverPath = configuration["Consul:ServerPath"];

            var client = new ConsulClient(c =>
            {
                c.Address = new Uri($"{serverPath}"); /*"http://192.168.2.72:8500/"*/
                c.Datacenter = "dcl";
                c.Token = "0ed48bc9-c448-4478-881c-10464dda5972";
            });

            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID =$"{name}:{ip}:{port}",/*"service" + Guid.NewGuid(), //唯一的*/
                Name = name, //名称
                Address = ip,
                Port = port,
                Tags = new string[] {weight.ToString()}, //标签
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(12),
                    HTTP = $"http://{ip}:{port}{checkPath}", ///*/api/identity/HealthCheck*/
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(50)
                }
            });;
        }

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app,
            IHostApplicationLifetime lifetime, ServiceEntity serviceEntity)
        {
            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri($"{serviceEntity.ServerPath}");
                //x.Token = serviceEntity.Token;
            }); //请求注册的 Consul 地址

            var httpCheck = new AgentServiceCheck
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(30), 
                Interval = TimeSpan.FromSeconds(10), 
                HTTP = $"http://{serviceEntity.IP}:{serviceEntity.Port}{serviceEntity.CheckPath}",
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration
            {
                Checks = new[] {httpCheck},
                ID = $"{serviceEntity.ServiceName}:{serviceEntity.IP}:{serviceEntity.Port}",
                Name = serviceEntity.ServiceName,
                Address = serviceEntity.IP,
                Port = serviceEntity.Port,
                Tags = new[]
                    {$"urlprefix-/{serviceEntity.ServiceName}"} //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait(); //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait(); //服务停止时取消注册
            });

            return app;
        }
    }
}
