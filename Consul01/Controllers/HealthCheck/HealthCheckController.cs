using Microsoft.AspNetCore.Mvc;

namespace Consul01.Controllers.HealthCheck
{
    /// <summary>
    /// 心跳检查
    /// </summary>
    [Route("api/Consul01/HealthCheck")]
    public class HealthCheckController  : ControllerBase
    {
        /// <summary>
        /// 心跳检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public OkResult HealthCheck()
        {
            return Ok();
        }
    }
}
