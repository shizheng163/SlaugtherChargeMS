using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    ///  屠宰加工前台显示节点
    /// </summary>
    public class ButcherDetail
    {
        public int Id;
        /// <summary>
        /// 收支类型
        /// </summary>
        public string Type;
        /// <summary>
        /// 收支原因
        /// </summary>
        public string Reason;

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TransTime;
        /// <summary>
        /// 录入时间
        /// </summary>
        public string EnterTime;
        /// <summary>
        /// 交易对象
        /// </summary>
        public string TranscationObject;
        /// <summary>
        /// 变更数量
        /// </summary>
        public int UpdateNum;
        /// <summary>
        /// 单个金额/支出金额
        /// </summary>
        public int SingleAmount;
        /// <summary>
        /// 总收入/总支出
        /// </summary>
        public string TotalSpend;
    }
}
