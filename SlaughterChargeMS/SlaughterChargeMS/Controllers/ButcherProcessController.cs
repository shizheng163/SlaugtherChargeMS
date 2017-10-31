/*
 * 单元名称:屠宰贩卖进货控制器
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
    public class ButcherProcessController : BaseController
    {
        readonly IButcherProcessService _butcherService = new ButcherProcessService();

        public ActionResult Statistics()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult DetailView(string Range, string ReasonCode, int Type)
        {
            ViewBag.Range = Range;
            ViewBag.ReasonCode = ReasonCode;
            ViewBag.Type = Type;
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 添加一次屠宰交易信息
        /// </summary>
        /// <returns></returns>
        public ContentResult AddButcherProcess(DB_ButcherProcess model)
        {
            string retMsg = string.Empty;
            try
            {
                _butcherService.Add(model, out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }

        public JsonResult QueryButcherDetailWithPager(SearchButcherDetailFilter filter)
        {
            var res = _butcherService.QueryButcherDetailWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询统计数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult QueryButcherStatiticsWithPager(SearchButStatisFilter filter)
        {
            var res = _butcherService.QueryButcherStatisWithPagerWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询统计视图
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryButcherStatiticsViewWithPager(SearchButcherStatisViewFilter filter)
        {
            var res = _butcherService.QueryButcherStatisViewWithPagerWithPager(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 删除一批交易信息
        /// </summary>
        /// <returns></returns>
        public ContentResult DeleteButcherDetailInfo(string IdList)
        {
            string retMsg = string.Empty;
            try
            {
                var list = IdList.Split(',').ToList();
                if (list.Count() == 0)
                    return Content("请选择要删除的信息！");
                var dList = from p in list select Convert.ToInt32(p);
                _butcherService.DeleteDetail(dList.ToList(), out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }


    }
}
