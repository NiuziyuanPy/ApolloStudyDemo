using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkywalkingWebApi_1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<string> GetAsync()
        {
            var client = new HttpClient();
            await client.GetStringAsync("http://192.168.2.96:6012/api/values/1");
            await client.GetStringAsync("http://192.168.2.96:6013/api/values/1");

            return await client.GetStringAsync("http://192.168.2.96:6012/api/values");
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "docker_webapi_nzy6011";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
