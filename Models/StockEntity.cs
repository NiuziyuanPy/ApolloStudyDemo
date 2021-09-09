using System;

namespace Models
{
    public class StockEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int StockCount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

    }
}
