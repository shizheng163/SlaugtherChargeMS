/*
 * 单元名称:拖欠客户账目管理
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
    public class CreditController : BaseController
    {
        readonly ICreditDetailService _CreditDetailService = new CreditDetailService();

        public ActionResult CreditUser()
        {
            return View();
        }

        public ActionResult Detail(int UserId = 0)
        {
            ViewBag.UserId = UserId;
            return View();
        }

        public JsonResult QueryArrearsUserInfo(SearchArrearsUserInfoFilter filter)
        {
            var res = _CreditDetailService.QueryUserInfoArrearsWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueryArrearsDetail(SearchArrearsDetailFilter filter)
        {
            var res = _CreditDetailService.QueryArrearsDetailWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 得到进货批次的下一个编号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        /// <summary>
        /// 添加一次进货批次信息
        /// </summary>
        /// <returns></returns>
        public ContentResult AddModel(DB_CreditDetail model)
        {
            string retMsg = string.Empty;
            try
            {
                _CreditDetailService.Add(model, out retMsg);
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
                _CreditDetailService.Delete(dList.ToList(), out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }
    }
}
