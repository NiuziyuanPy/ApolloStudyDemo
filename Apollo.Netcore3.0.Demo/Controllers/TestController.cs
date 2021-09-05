using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Apollo.Netcore3._0.Demo.Controllers
{
    [Route("api/Apollo/[controller]")]
    public class TestController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public TestController(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("Get")]
        public WeatherForecast Get()
        {
            var dbValue = _configuration.GetValue<string>("ConfigurationDbConnection");
            var textValue = _configuration.GetValue<string>("UserTest");
            //var all = _configuration.Get<object>();

            _logger.LogInformation("apollo test 请求记录");

            return new WeatherForecast
            {
                ApolloName = "test_server",
                DbConfigName = "ConfigurationDbConnection",
                DbConfigInfo = dbValue,
                TextConfigName = "UserTest",
                TextConfigInfo = textValue,
                Date = DateTime.Now,
            };
        }
    }
}
