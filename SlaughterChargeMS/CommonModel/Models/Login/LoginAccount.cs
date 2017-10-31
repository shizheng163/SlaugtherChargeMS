/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 15:36:58
*  文件名：LoginAccount
*  说明： 
*───────────────────────────────────
*  V0.01 2017/7/21 15:36:58 史正 初版
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 保存当前登录的用户信息
    /// </summary>
    public class LoginAccount
    {

        /// <summary>
        /// 当前登录的用户账户Id
        /// </summary>
        public int loginUserId;

        public string loginName;

        /// <summary>
        /// 当前用户类型 
        /// </summary>
        public int loginType;

        /// <summary>
        /// 登录姓名
        /// </summary>
        public string name;

        /// <summary>
        /// 菜单编码集合
        /// </summary>
        public string menusCode;
    }
}
