/*
 * 单元名称:用户管理控制器
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonModel.Filter;
using CommonModel;
using MSBLL;
using MSIBLL;
namespace SlaughterChargeMS.Controllers
{
    public class ManagerController : BaseController
    {
        readonly IManagerService _managerService = new ManagerService();

        public ActionResult Manager()
        {
            return View();
        }

        public JsonResult QueryUserInfoWithPager(PagingFilter filter)
        {
            var res = _managerService.QueryAllManagerWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        /// <summary>
        /// 添加一次进货批次信息
        /// </summary>
        /// <returns></returns>
        public ContentResult AddUser(DB_Manager model)
        {
            string retMsg = string.Empty;
            try
            {
                model.OperatorName = string.Format("{0}({1})", loginAccount.name, loginAccount.loginName);
                model.OperatorTime = System.DateTime.Now.ToString("F");
                _managerService.AddManager(model, out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }

        [HttpPost]
        /// <summary>
        /// 添加一次进货批次信息
        /// </summary>
        /// <returns></returns>
        public ContentResult Update(DB_Manager model)
        {
            string retMsg = string.Empty;
            try
            {
                var oldModel = _managerService.GetManager(model.Id);
                oldModel.Name = model.Name;
                if (oldModel.LoginName == "admin" && oldModel.LoginName != model.LoginName)
                    throw new Exception("内置管理员不允许修改登录名");
                oldModel.LoginName = model.LoginName;
                oldModel.OperatorName = string.Format("{0}({1})", loginAccount.name, loginAccount.loginName);
                oldModel.OperatorTime = System.DateTime.Now.ToString("F");
                _managerService.UpdateManager(oldModel, out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }

        [HttpPost]
        /// <summary>
        /// 删除一批交易信息
        /// </summary>
        /// <returns></returns>
        public ContentResult Delete(string IdList)
        {
            string retMsg = string.Empty;
            try
            {
                var list = IdList.Split(',').ToList();
                if (list.Count() == 0)
                    return Content("请选择要删除的信息！");
                if (list.Contains(loginAccount.loginUserId.ToString()))
                {
                    return Content("您不能删除自己的账户!");
                }
                var dList = from p in list select Convert.ToInt32(p);
                _managerService.Delete(dList.ToList(), out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }

        [HttpPost]
        /// <summary>
        /// 删除一批交易信息
        /// </summary>
        /// <returns></returns>
        public ContentResult ResetPassword(int UserId)
        {
            string retMsg = string.Empty;
            try
            {
                _managerService.ResetUserPassword(UserId);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }
    }
}
