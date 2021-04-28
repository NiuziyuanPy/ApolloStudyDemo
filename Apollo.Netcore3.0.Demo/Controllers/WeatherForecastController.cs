using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Apollo.Netcore3._0.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
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
