using Dapper;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace Cap02.Controllers
{
    [ApiController]
    [Route("Api/Stock/CapOrder")]
    public class CapStockController : ControllerBase
    {
        private readonly ICapPublisher _capBus;

        public CapStockController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        /// <summary>
        /// 获取库存信息 ADO方式
        /// </summary>
        /// <param name="productNo">产品编号</param>
        /// <returns></returns>
        [HttpGet("GetStock")]
        public StockEntity GetStock(string productNo)
        {
            StockEntity stock;

            try
            {
                using var connection = new MySqlConnection(AppDbContext.ConnectionString);

                stock = connection.QueryFirst<StockEntity>(
                    @"SELECT * from Stock WHERE ProductNo = @ProductNo; ", new
                    {
                        ProductNo = productNo
                    });
            }
            catch (Exception ex)
            {
                stock = new StockEntity();
            }

            return stock;
        }


        [NonAction]
        [CapSubscribe("Order.Create.Success")]
        public void UpdateStock(OrderEntity order)
        {
            //throw new Exception("扣减库存异常了");
            using var connection = new MySqlConnection(AppDbContext.ConnectionString);
            // 模拟扣减库存
            using var transaction = connection.BeginTransaction(_capBus, false);
            try
            {
                var product = connection.QueryFirst<StockEntity>(
                    @"SELECT * from Stock WHERE ProductNo = @ProductNo; ", new
                    {
                        order.ProductNo
                    },
                    (IDbTransaction) transaction.DbTransaction);
                product.StockCount -= order.Count;

                connection.Execute(
                    @"UPDATE `Stock` SET `ProductNo` = @ProductNo, `StockCount` = @StockCount, `UpdateDate` = @UpdateDate WHERE `Id` = @Id;",
                    product,
                    (IDbTransaction) transaction.DbTransaction);

                // 可以继续向下发布流程，比如库存扣减成功，下一步到物流服务进行相关处理，可以继续发布消息
                // _capPublisher.Publish();
                transaction.Commit();

                Console.WriteLine(order.OrderNo);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }

        }
    }
}
