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
                EnterpriseWeChatRead = _configuration["EnterpriseWeChatRead"]
            };
        }

    }

    public class ConfigDto
    {
        public string UserTest { get; set; }

        public string EnterpriseWeChatRead { get; set; }
    }
}
