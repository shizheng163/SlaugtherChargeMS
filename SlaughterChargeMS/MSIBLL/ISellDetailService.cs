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
    public partial interface ISellDetailService : IBaseService<DB_SellDetail>
    {
        /// <summary>
        /// 添加一条交易信息
        /// </summary>
        void AddSellDetail(DB_SellDetail model,out string retMsg);

        /// <summary>
        /// 删除
        /// </summary>
        void DeleteDetail(List<int> IdList, out string retMsg);
        /// <summary>
        /// 删除父类批次下的所有交易细节 包括进货交易
        /// </summary>
        /// <param name="FK_Sell"></param>
        /// <param name="retMsg"></param>
        void Delete(int FK_Sell, out string retMsg);

    }
}
