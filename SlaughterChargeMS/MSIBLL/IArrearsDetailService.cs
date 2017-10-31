/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017年8月22日22:57:30
*  文件名：IArrearsDetailService
*  说明： 欠款管理的业务层接口
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
    public partial interface IArrearsDetailService : IBaseService<DB_ArrearsDetail>
    {
        void Add(DB_ArrearsDetail model, out string retMsg);

        void Update(DB_ArrearsDetail model, out string retMsg);

        void Delete(List<int> list, out string retMsg);
        DB_ArrearsDetail GetModel(int Id);

        /// <summary>
        /// 得到某个用户的欠款金额
        /// </summary>
        int GetUserArrearsMoney(int UserId);
        /// <summary>
        /// 按人员查看欠款信息
        /// </summary>
        /// <returns></returns>
        PagingResults<ArrearsUserInfo> QueryUserInfoArrearsWithPager(SearchArrearsUserInfoFilter filter);

        PagingResults<ArrearsDetail> QueryArrearsDetailWithPager(SearchArrearsDetailFilter filter);
    }
}
