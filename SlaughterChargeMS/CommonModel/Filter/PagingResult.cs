/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/25 17:04:59
*  文件名：PagingResult
*  说明： 分页后的结果集
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Filter
{
    /// <summary>
    /// 基于分页效果的结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagingResults<T>
    {
        public PagingResults()
        {
            Results = null;
        }

        public PagingResults(IEnumerable<T> collection)
        {
            Results = collection;
        }

        public PagingResults(IQueryable<T> collection) : this(collection.ToList())
        {
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public long TotalSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public long CurrentPageNo { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
