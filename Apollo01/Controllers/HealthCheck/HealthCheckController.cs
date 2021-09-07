using Microsoft.AspNetCore.Mvc;

namespace Apollo01.Controllers.HealthCheck
{
    /// <summary>
    /// 心跳检查
    /// </summary>
    [Route("api/Apollo/HealthCheck")]
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
