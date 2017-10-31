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
    public partial interface IUserInfoService : IBaseService<DB_UserInfo>
    {
        void Add(DB_UserInfo user, out string mess);

        DB_UserInfo GetModel(int id);
        DB_UserInfo GetModel(string userNo);
        /// <summary>
        /// 编号不允许更改
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mess"></param>
        void Update(DB_UserInfo user, out string mess);

        /// <summary>
        /// 得到下一个用户编号
        /// </summary>
        /// <returns></returns>
        string GetNextUserNo();

        PagingResults<UserShowInfo> QueryUserInfoWithPager(SearchUserInfoFilter filter);

        void Delete(List<int> list, out string retMsg);
    }
}
