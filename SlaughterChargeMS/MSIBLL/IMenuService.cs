/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 11:34:56
*  文件名：IMenuService
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
namespace MSIBLL
{
    public partial interface IMenuService : IBaseService<DB_Menus>
    {
        List<DB_Menus> GetAllMenus(int LoginType);

        /// <summary>
        /// 得到某一个模块下的菜单几个
        /// </summary>
        /// <param name="MenusCode">该模块的菜单编码</param>
        List<DB_Menus> GetAllMenus(string MenusCode);

        /// <summary>
        /// 得到某类用户下的菜单集合
        /// </summary>
        /// <param name="LoginType"></param>
        /// <returns></returns>
        string GetMenusCodes(int LoginType);
        
    }
}
