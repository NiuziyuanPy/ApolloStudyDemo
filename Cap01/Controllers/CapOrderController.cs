using Dapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Models;
using MySqlConnector;
using System;
using System.Data;
using Newtonsoft.Json.Linq;

namespace Cap01.Controllers
{
    [ApiController]
    [Route("Api/Order/CapOrder")]
    public class CapOrderController : ControllerBase
    {
        private readonly ICapPublisher _capBus;

        public CapOrderController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        /// <summary>
        /// 新增订单 ADO方式
        /// </summary>
        /// <returns></returns>
        [HttpGet("AddOrder")]
        public string AddOrder()
        {
            OrderEntity orderEntity = new OrderEntity
            {
                OrderNo = Guid.NewGuid().ToString(),
                ProductName = "商品01",
                ProductNo= "Product01",
                Count = 5,
                CreateDate = DateTime.Now
            };

            //数据库
            using var connection = new MySqlConnection(AppDbContext.ConnectionString);
            // 开启本地事务
            using var transaction = connection.BeginTransaction(_capBus, false);

            try
            {
                //业务逻辑 1，2，3

                //新增订单
                connection.Execute(
                    @$"INSERT INTO `Order_Test`(`OrderNo`, `ProductName`, `ProductNo`, `Count`, `CreateDate`) VALUES(@OrderNo,@ProductName, @ProductNo,@Count, @CreateDate);",
                    orderEntity,
                    (IDbTransaction) transaction.DbTransaction);

                _capBus.Publish("Order.Create.Success", orderEntity);


                //throw new Exception("手动异常");

                transaction.Commit();

                return "新增订单成功";
            }
            catch (Exception ex)
            {
                transaction.Rollback();//回滚
                Console.WriteLine("-------------------------------失败报错-------------------------------------------------");
                throw;
            }
        }


        // =============  Publisher =================
        /// <summary>
        /// 补偿事务测试
        /// </summary>
        [HttpGet("PublisherTest")]
        public void PublisherTest()
        {
            using var connection = new MySqlConnection(AppDbContext.ConnectionString);
            // 开启本地事务
            using var transaction = connection.BeginTransaction(_capBus, false);
            _capBus.Publish("place.order.qty.deducted", new OrderEntity
                {
                    OrderNo = Guid.NewGuid().ToString(),
                    ProductName = "商品01",
                    ProductNo = "Product01",
                    Count = 5,
                    CreateDate = DateTime.Now
                },
                "place.order.mark.status");
            transaction.Commit();
        }

        // publisher using `callbackName` to subscribe consumer result
        [NonAction]
        [CapSubscribe("place.order.mark.status")]
        public void MarkOrderStatus(string param)
        {
            Console.WriteLine(param);
            if (1==1)
            {
                //param.Count = 10;

                Console.WriteLine("mark order status to succeeded");
            }
            else
            {
                Console.WriteLine("mark order status to failed");
            }
        }

        // =============  Consumer 补偿事务 ===================
        [NonAction]
        [CapSubscribe("place.order.qty.deducted")]
        public OrderEntity DeductProductQty(string param)
        {
            Console.WriteLine(param);
            Console.WriteLine("business logic"); 

            return new OrderEntity
            {
                OrderNo = Guid.NewGuid().ToString(),
                ProductName = "商品01",
                ProductNo = "Product01",
                Count = 5,
                CreateDate = DateTime.Now
            };
        }




}
}
