/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017年7月21日15:06:33
*  文件名：CommonController
*  说明： 通用接口控制器
*───────────────────────────────────
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonModel.Models;
using MSBLL;
using MSIBLL;
using CommonModel;
namespace SlaughterChargeMS.Controllers
{
    public class CommonController : BaseController
    {
        readonly IDictionaryCategoryService _dictCategoryservice = new DictionaryCategoryService();
        readonly IDictionaryInfoService _dictInfoservice = new DictionaryInfoService();
        public ContentResult GetLoginName()
        {
            return Content(LoginName);
        }

        #region 获取菜单Json
        public JsonResult GetLoginMenusJson()
        {
            Dictionary<string, List<LoginMenu>> dict = new Dictionary<string, List<LoginMenu>>();
            List<LoginMenu> menus = new List<LoginMenu>();
            LoginAccount acc = this.loginAccount;
            if (acc == null)
            {
                dict.Add("menus", menus);
                RedirectToAction("Login", "Index");
            }
            if (acc != null && acc.menusCode != null && acc.menusCode.Length > 0)
            {
                string menuStr = "," + acc.menusCode + ",";
                foreach (var m in this.GetLoginMenuList(loginAccount.loginType))
                {
                    if (menuStr.Contains("," + m.menuid + ","))
                        menus.Add(m);
                }
                GetLoginMenusRemoveNotPower(menuStr, menus);
            }
            else
            {
                dict.Add("menus", menus);
                RedirectToAction("Login", "Index");
            }
            dict.Add("menus", menus);
            return Json(dict, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 去除没有权限的菜单
        /// </summary>
        /// <param name="MenuCodes">用户的菜单编码</param>
        /// <param name="loginMenus">传入的登录菜单,可能是顶级菜单也可以是子级的。一开始传入的是顶级，排除顶级菜单下不属于用户的数据。</param>
        private void GetLoginMenusRemoveNotPower(string MenuCodes, List<LoginMenu> loginMenus)
        {
            for (int i = 0; i < loginMenus.Count(); ++i)
            {
                var info = loginMenus[i];
                if (!MenuCodes.Contains("," + info.menuid + ","))
                {
                    loginMenus.Remove(info);
                    i--;//移除后迭代器应减小
                }
                else
                    GetLoginMenusRemoveNotPower(MenuCodes, info.menus);
            }
        }
        #endregion

        #region 修改密码
        // 修改密码
        [HttpPost]
        public ActionResult ModPwd()
        {
            string retMsg = "";
            try
            {
                IManagerService _managerService = new ManagerService();
                bool ret = false;
                int loginTyp = 0;
                string loginCode = loginAccount.loginName;
                string oPwd = Request.Form["oldpwd"];
                string nPwd = Request.Form["newpwd"];
                string tStr = "";
                if (loginTyp == 0)
                {
                    ret = _managerService.SetPwd(loginCode, oPwd, nPwd, nPwd, out retMsg);
                    if (ret)
                        tStr = string.Format("修改用户[{0}]密码！-{1}", loginAccount.name, "成功");
                    else
                        tStr = string.Format("修改用户[{0}]密码！-{1}：{2}", loginAccount.name, "失败", retMsg);
                }
            }
            catch (Exception e)
            {
                retMsg = string.Format("发生错误:{0}", e.Message);
            }
            return Content(retMsg);
        }
        #endregion

        #region 字典相关
        /// <summary>
        /// 根据字典分类的名称获得所有字典数据JSON数据
        /// </summary>
        /// <param name="DictionaryCategoryName">字典分类名称</param>
        /// 新加床位类型，单独调用床位表数据
        /// <returns></returns>
        public JsonResult GetComboBoxValue(string name)
        {
            string code = _dictCategoryservice.GetCodeByName(name);
            return GetComboBoxValueByCode(code);
        }
        //通过大类编号查询
        public JsonResult GetComboBoxValueByCode(string name)
        {
            string code = name;
            if (!string.IsNullOrEmpty(code))
            {
                List<DB_DictionaryInfo> result;
                result = _dictInfoservice.GetALLByFKDictionaryCategory(code);
                if (null != result)
                {
                    var json = from d in result
                               select new
                               {
                                   Code = d.Code,
                                   Name = d.Name,
                               };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
