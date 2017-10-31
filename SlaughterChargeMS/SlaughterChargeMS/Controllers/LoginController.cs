using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIBLL;
using MSBLL;
using SlaughterChargeMS.Config;
namespace SlaughterChargeMS.Controllers
{
    public class LoginController : Controller
    {
        #region 变量声明区域
        IManagerService _managerService = new ManagerService();
        IUserInfoService _userInfoService = new UserInfoService();
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Loginout()
        {
            return RedirectToAction("Index", "Login");
        }
        /// <summary>
        /// 网站后台登录界面
        /// </summary>
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            //ViewBag.Message = string.Format("your username is:{0},your password is:{1}", username, password);
            var Manager = _managerService.GetManager(username, password);
            if (Manager == null)//不是管理员
            {
                ViewBag.Message = "用户名不存在或者密码错误！";
                return View();
            }
            else
            {
                _managerService.SetManagerMenus(Manager);
                BaseController.SetLogin(Manager, Session, Server);
                string url = "~/Main.htm";
                return Redirect(url);
            }
        }
    }
}
