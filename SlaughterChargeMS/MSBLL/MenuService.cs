/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 14:52:17
*  文件名：MenuService
*  说明： 
*───────────────────────────────────
*  V0.01 2017/7/21 14:52:17 史正 初版
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSIBLL;
using CommonModel;
namespace MSBLL
{
    public class MenuService : BaseService<DB_Menus>, IMenuService
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = db.MenusRepository;
        }

        public List<DB_Menus> GetAllMenus(int LoginType)
        {
            return LoadEntities(p => p.MenuType == LoginType).ToList();
        }

        public string GetMenusCodes(int LoginType)
        {
            var list = GetAllMenus(LoginType);
            var strList = from p in list select p.PK_Code;
            return string.Join(",", strList);
        }


        public List<DB_Menus> GetAllMenus(string MenusCode)
        {
            return LoadEntities(p => p.PK_Code.StartsWith(MenusCode)).ToList();
        }
    }
}
