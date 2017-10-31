/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 11:34:56
*  文件名：IManagerService
*  说明： 
*───────────────────────────────────
*  V0.01 2017/7/21 11:34:56 史正 初版
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
    public partial interface IManagerService : IBaseService<DB_Manager>
    {
        /// <summary>
        /// 得到管理员的数据库模型
        /// </summary>
        DB_Manager GetManager(string UserId, string password);

        DB_Manager GetManager(int UserId);


        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pwd">旧密码</param>
        /// <param name="newpwd">新密码</param>
        /// <param name="newpwdok">确认密码</param>
        /// <returns></returns>
        bool SetPwd(string userID, string pwd, string newpwd, string newpwdok, out string errorMsg);

        void AddManager(DB_Manager manager,out string mess);

        void Delete(List<int> IdList, out string mess);

        void ResetUserPassword(int UserId);

        void UpdateManager(DB_Manager manager, out string mess);

        PagingResults<ManagerNode> QueryAllManagerWithPager(PagingFilter filter);

        /// <summary>
        /// 设置菜单权限
        /// 当数据库中只有admin时 admin权限为全部,否则admin权限只为系统设置-用户管理
        /// </summary>
        void SetManagerMenus(DB_Manager manager);
    }
}
