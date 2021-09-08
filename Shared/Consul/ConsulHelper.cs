using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using Tect.Shared;

namespace Shared
{
    public static class ConsulHelper
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app,
            IHostApplicationLifetime lifetime, ServiceEntity serviceEntity)
        {
            var consulClient = new ConsulClient(x =>
            {
                x.Address = new Uri($"{serviceEntity.ServerPath}");
                x.Token = serviceEntity.Token;
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
