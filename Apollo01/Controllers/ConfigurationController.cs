using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Apollo01.Controllers
{
    [ApiController]
    [Route("api/Apollo/Configuration")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetTestConfig")]
        public ConfigDto GetTestConfig()
        {
            //var textValue = _configuration.GetValue<string>("UserTest");

            return new ConfigDto
            {
                UserTest = _configuration["UserTest"],
                EnterpriseWeChatRead = _configuration["EnterpriseWeChatRead"],
                ServerPath = _configuration["Consul:ServerPath"],
                CheckPath = _configuration["Consul:CheckPath"],
                Token = _configuration["Consul:Token"]
            };
        }

    }

    public class ConfigDto
    {
        public string UserTest { get; set; }

        public string EnterpriseWeChatRead { get; set; }

        public string ServerPath { get; set; }

        public string CheckPath { get; set; }

        public string Token { get; set; }
    }
}
