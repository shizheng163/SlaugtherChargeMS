/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/25 17:03:14
*  文件名：PagingFilter
*  说明： 用于指定分页效果
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Filter
{
    /// <summary>
    /// 用于指定分页效果
    /// </summary>
    [Serializable]
    public class PagingFilter
    {
        public int PageSize
        {
            get
            {
                return rows;
            }
        }
        public int PageNo
        {
            get
            {
                return page;
            }
        }
        public int rows { get; set; }
        public int page { get; set; }
    }
}
