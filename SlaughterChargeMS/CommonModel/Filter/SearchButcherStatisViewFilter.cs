using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Filter
{
    /// <summary>
    /// 查看屠宰统计视图 详情过滤器
    /// </summary>
    public class SearchButcherStatisViewFilter : PagingFilter
    {
        public string Range { get; set; }
    }
}
