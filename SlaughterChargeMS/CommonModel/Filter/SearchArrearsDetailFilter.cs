using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Filter
{
    /// <summary>
    /// 查询贩卖批次过滤器
    /// </summary>
    public class SearchArrearsDetailFilter : PagingFilter
    {
        /// <summary>
        ///查询范围前缀 YYYY-MM-DD
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 0-欠款信息 1-还款信息 2-全部
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// 0代表全部
        /// </summary>
        public int FK_UserInfo { get; set; }
    }
}
