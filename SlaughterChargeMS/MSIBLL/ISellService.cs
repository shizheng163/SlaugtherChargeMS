/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017年7月31日15:53:27
*  文件名：IUserInfoService
*  说明： 微信用户扩展信息操作的业务层
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
    public partial interface ISellService : IBaseService<DB_Sell>
    {
        /// <summary>
        /// 查询贩卖批次详情
        /// </summary>
        PagingResults<SellBatchInfo> QuerySellBatchInfoList(SearchSellBatchFilter filter);


        /// <summary>
        /// 查询贩卖批次详情列表（DB_SellDetail）
        /// </summary>
        PagingResults<SellBatchDetailInfo> QuerySellBatchDetaiInfoList(SearchSellBatchDetailFilter filter);

        /// <summary>
        /// 得到下一个进货批次编号
        /// </summary>
        int GetNextSellBatchNumber();

        /// <summary>
        /// 添加
        /// </summary>
        void Add(DB_Sell model, out string mess);

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="SellId"></param>
        /// <returns></returns>
        SellBatchInfo QuerySignleBatchInfo(int SellId);

        void Delete(List<int> list, out string retMsg);

    }
}
