using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonModel.Models;
using CommonModel;
using MSIBLL;
using MSBLL;
using SlaughterChargeMS.Filters;
namespace SlaughterChargeMS.Controllers
{
    [Login]
    public class BaseController : Controller
    {
        #region 变量声明
        readonly static IMenuService _menuService = new MenuService();
        #endregion

        #region 全局属性
        protected LoginAccount loginAccount
        {
            get
            {
                if (Session == null)
                    return null;
                else
                    return Session["LoginAccount"] as LoginAccount;
            }
        }
        /// <summary>
        /// 当前可访问的控制器名称集合
        /// </summary>
        protected IList<string> Functions
        {
            get
            {
                if (Session == null)
                    return null;
                return Session["Functions"] as IList<string>;       
            }
        }

        protected string LoginName {
            get {
                return loginAccount.name;
                }
        }
        #endregion

        #region 过滤器方法
        /// <summary>
        /// 重写Login过滤器的虚函数  该函数在Action执行前调用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //如果当前将要访问Common控制器的GetLoginMenusJson方法
            if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Common" && filterContext.ActionDescriptor.ActionName == "GetLoginMenusJson")
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            //当前未登录
            if (this.loginAccount == null)
            {
                string url = string.Format("http://{0}{1}", this.Request.ServerVariables["HTTP_HOST"].ToString(), Request.Url.PathAndQuery);
                filterContext.Result = RedirectToRoute("Default", new { Controller = "Login", Action = "Index", u = url });
            }
            else
            {
                string path = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                path = path.ToLower();
                ///如果访问的是Common控制器则不做过滤
                if (path == "common")
                {
                    base.OnActionExecuting(filterContext);
                    return;
                }
                //if (!this.Functions.Contains(path))
                //    filterContext.Result = RedirectToRoute("Default", new { Controller = "Login", Action = "Index" });
            }
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region 控制器方法

        #region 获取菜单信息
        /// <summary>
        /// 根据登录类型获取菜单权限
        /// </summary>
        /// <param name="loginType">1-微信后台登陆 2-教师登录 3-学生登录 4-未认证用户登录</param>
        /// <returns></returns>
        protected List<LoginMenu> GetLoginMenuList(int loginType)
        {
            List<LoginMenu> list = new List<LoginMenu>();
            List<DB_Menus> dbMenus = _menuService.GetAllMenus(loginType);
            if (loginType == 2 || loginType == 3)
                dbMenus.AddRange(_menuService.GetAllMenus("040"));
            dbMenus = dbMenus.OrderBy(p => p.Orders).ToList();
            List<DB_Menus> topMenus = dbMenus.FindAll(p => string.IsNullOrEmpty(p.FK_PCode));
            //生成顶级菜单列表
            foreach (DB_Menus dm in topMenus)
            {
                list.Add(new LoginMenu()
                {
                    menuid = dm.PK_Code,
                    menuname = dm.Name,
                    icon = dm.IconCls,
                    url = dm.Url,
                    menus = new List<LoginMenu>()
                });
            }
            //循环添加多级子节点
            AddSubMenuList(list, dbMenus);
            return list;
        }

        /// <summary>
        /// 添加子菜单
        /// </summary>
        private void AddSubMenuList(List<LoginMenu> menus, List<DB_Menus> dbMenus)
        {
            foreach (var menu in menus)
            {
                List<DB_Menus> subMenus = dbMenus.FindAll(p => p.FK_PCode == menu.menuid);
                foreach (var dm in subMenus)
                {
                    menu.menus.Add(new LoginMenu()
                    {
                        menuid = dm.PK_Code,
                        menuname = dm.Name,
                        icon = dm.IconCls,
                        url = dm.Url,
                        menus = new List<LoginMenu>()
                    });
                }
                AddSubMenuList(menu.menus, dbMenus);
            }
        }
        #endregion

        #region 设置登录信息
        public static void SetLogin(DB_Manager manager, HttpSessionStateBase Session, HttpServerUtilityBase Server)
        {
            if (Session == null)
                return;
            if (manager == null)
            {
                Session["LoginAccount"] = null;
                Session["Functions"] = null;
            }
            else
            {
                LoginAccount Account = new LoginAccount();
                Account.loginType = 1;
                Account.loginUserId = manager.Id;
                Account.loginName = manager.LoginName;
                Account.name = manager.Name;
                Account.menusCode = manager.Menus;
                Session["LoginAccount"] = Account;
                if (string.IsNullOrEmpty(Account.menusCode))
                {
                    Session["Functions"] = new List<string>();
                    return;
                }
                Session["Functions"] = GetCurFunctions(Account.loginType, Account.menusCode);
                Session.Timeout = 600;
            }
        }

        public static void SetLogin(DB_UserInfo manager, HttpSessionStateBase Session, HttpServerUtilityBase Server)
        {
            //默认不提供用户登录功能只提供管理员登录功能
            //if (Session == null)
            //    return;
            //if (manager == null)
            //{
            //    Session["LoginAccount"] = null;
            //    Session["Functions"] = null;
            //}
            //else
            //{
                
            //    LoginAccount Account = new LoginAccount();
            //    Account.name = manager.Name;
            //    Account.WXUserId = manager.Id;
            //    Account.userId = manager.OpenId;
            //    Account.loginType = 4;
            //    Account.menusCode = _menuService.GetMenusCodes(4);
            //    Session["LoginAccount"] = Account;
            //    if (string.IsNullOrEmpty(Account.menusCode))
            //    {
            //        Session["Functions"] = new List<string>();
            //        return;
            //    }
            //    Session["Functions"] = GetCurFunctions(Account.loginType, Account.menusCode);
            //    Session.Timeout = 600;
            //}
        }
        /// <summary>
        /// 通过登录类型 获取当前菜单所可以访问的控制器名称集合
        /// </summary>
        /// <param name="LoginType"></param>
        /// <returns>Login,Student,Teacher等等</returns>
        private static List<string> GetCurFunctions(int LoginType,string CurMenus)
        {
            IMenuService _menuService = new MenuService();
            var dbMenus = _menuService.GetAllMenus(LoginType);
            var result = new List<string>();
            CurMenus = "," + CurMenus + ",";
            foreach (var info in dbMenus)
            {
                if (CurMenus.Contains("," + info.PK_Code + ","))
                {
                    if (!string.IsNullOrEmpty(info.Url))
                        result.Add(info.Url.Substring(0, info.Url.IndexOf("/")).ToLower());
                }
            }
            return result;
        }
        #endregion

        #endregion

    }
}
