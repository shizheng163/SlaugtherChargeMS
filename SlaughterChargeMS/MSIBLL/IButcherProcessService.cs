/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017年7月31日15:53:27
*  文件名：IUserInfoService
*  说明： 屠宰加工管理 业务层接口
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel;
using CommonModel.Filter;
using CommonModel.Models;
namespace MSIBLL
{
    public partial interface IButcherProcessService : IBaseService<DB_ButcherProcess>
    {
        void Add(DB_ButcherProcess model, out string retMsg);

        /// <summary>
        /// 查询屠宰加工详情
        /// </summary>
        PagingResults<ButcherDetail> QueryButcherDetailWithPager(SearchButcherDetailFilter filter);
        /// <summary>
        /// 删除交易细节
        /// </summary>
        void DeleteDetail(List<int> list, out string retMsg);

        /// <summary>
        /// 统计信息查询
        /// </summary>
        /// <returns></returns>
        PagingResults<ButcherStatis> QueryButcherStatisWithPagerWithPager(SearchButStatisFilter filter);

        /// <summary>
        /// 统计视图详情原因查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        PagingResults<ButcherStatisView> QueryButcherStatisViewWithPagerWithPager(SearchButcherStatisViewFilter filter);

    }
}
