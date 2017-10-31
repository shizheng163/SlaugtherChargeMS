using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 屠宰加工信息显示节点
    /// </summary>
    public class ButcherStatis
    {
        /// <summary>
        /// 前台索引 
        /// </summary>
        public int Index;

        /// <summary>
        /// 统计区间
        /// </summary>
        public string Range;
        /// <summary>
        /// 收入金额
        /// </summary>
        public string IncomeMoney;
        /// <summary>
        /// 支出金额
        /// </summary>
        public string PayMoney;
        /// <summary>
        /// 盈利金额
        /// </summary>
        public string ProfitMoney;
    }
}
