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
    public class UserInfoController : BaseController
    {
        readonly IUserInfoService _userInfoService = new UserInfoService();

        public ActionResult UserInfo()
        {
            return View();
        }

        public JsonResult QueryUserInfoWithPager(SearchUserInfoFilter filter)
        {
            var res = _userInfoService.QueryUserInfoWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 得到进货批次的下一个编号
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNextUserNo()
        {
            try
            {
                string num = _userInfoService.GetNextUserNo();
                return Json(new { ret = true, data = num }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ret = false, errorMsg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        /// <summary>
        /// 添加一次进货批次信息
        /// </summary>
        /// <returns></returns>
        public ContentResult AddUser(DB_UserInfo model)
        {
            string retMsg = string.Empty;
            try
            {
                _userInfoService.Add(model, out retMsg);
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
        public ContentResult Update(DB_UserInfo model)
        {
            string retMsg = string.Empty;
            try
            {
                var oldModel = _userInfoService.GetModel(model.Id);
                oldModel.Name = model.Name;
                oldModel.Phone = model.Phone;
                oldModel.Company = model.Company;
                _userInfoService.Update(oldModel, out retMsg);
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
                var dList = from p in list select Convert.ToInt32(p);
                _userInfoService.Delete(dList.ToList(), out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }
    }
}
