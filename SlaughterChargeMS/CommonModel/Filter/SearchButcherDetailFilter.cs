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
    public class SearchButcherDetailFilter:PagingFilter
    {
        /// <summary>
        ///查询范围前缀 YYYY-MM-DD
        /// </summary>
        public string Range { get; set; }
        /// <summary>
        /// 0-收入 1-支出 2-全部
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// 0代表全部
        /// </summary>
        public string ReasonCode { get; set; }
    }
}
