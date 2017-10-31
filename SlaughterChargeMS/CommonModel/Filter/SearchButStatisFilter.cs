using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Filter
{
    /// <summary>
    /// 统计屠宰加工羊过滤器
    /// </summary>
    public class SearchButStatisFilter : PagingFilter
    {
        /// <summary>
        /// 类型 0-日 1-月 2-年
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 统计范围 YYYY-MM-DD 当为
        /// </summary>
        public string Range { get; set; }
    }
}
