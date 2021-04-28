using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Nancy.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SkywalkingWebApi_5.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var client = new HttpClient();
            var constructorString = "server=192.168.2.59;User Id=root;password=123qweasd,./;Database=UserInfo_Test";
            var myConnnect = new MySqlConnection(constructorString);
            myConnnect.Open();
            //var data = DateTime.Now.ToString();
            //var myCmd = new MySqlCommand("insert into User(`name`,agv) values('牛子元','99')", myConnnect);
            //var ss = myCmd.CommandText;
            //Console.WriteLine(myCmd.CommandText);
            //var sss = myCmd.ExecuteNonQuery();

            var myCmd1 = new MySqlCommand("SELECT * FROM `User` ", myConnnect);

            var user = myCmd1.ExecuteScalarAsync();


            return new string[] { $"{new JavaScriptSerializer().Serialize(user)}" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
