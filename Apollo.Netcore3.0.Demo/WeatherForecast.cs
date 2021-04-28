using System;

namespace Apollo.Netcore3._0.Demo
{
    public class WeatherForecast
    {
        public string ApolloName { get; set; }

        public string DbConfigName { get; set; }

        public string DbConfigInfo { get; set; }

        public string TextConfigName { get; set; }

        public string TextConfigInfo { get; set; }

        //public 

        public string Remark { get; set; } = "可以在apollo配置中心【http://192.168.2.96:8070/】修改发布，这边会实时获取";

        public DateTime Date { get; set; }
    }
}
