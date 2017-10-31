using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    ///  屠宰加工前台统计视图显示节点
    /// </summary>
    public class ButcherStatisView
    {
        public string ReasonCode;

        public string ReasonName;
        /// <summary>
        /// 变更数量
        /// </summary>
        public int UpdateNum;
        /// <summary>
        /// 收支金额
        /// </summary>
        public string TotalMoney;
    }
}
