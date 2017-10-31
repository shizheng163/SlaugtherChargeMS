using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 批次管理表格显示节点
    /// </summary>
    public class SellBatchInfo
    {
        public int Id;
        /// <summary>
        /// 批次编号
        /// </summary>
        public int BatchNumber;
        /// <summary>
        /// 进货日期
        /// </summary>
        public string QuantityDate;
        /// <summary>
        /// 交易对象
        /// </summary>
        public string TranscationObject;
        /// <summary>
        /// 贩卖类型 猪或者羊等等
        /// </summary>
        public string SellCategory;
        /// <summary>
        /// 进货数量
        /// </summary>
        public int QuantityNum;
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RemainderNum;
        /// <summary>
        /// 进货支出
        /// </summary>
        public double QuantitySpend;
        /// <summary>
        /// 其他支出
        /// </summary>
        public double OtherSpend;
        /// <summary>
        /// 当前收入
        /// </summary>
        public double CurIncome;
        /// <summary>
        /// 当前盈利
        /// </summary>
        public double CurProfit;
        /// <summary>
        /// 录入时间
        /// </summary>
        public string EnterTime;
        /// <summary>
        /// 经办人
        /// </summary>
        public string Operator;

    }
}
