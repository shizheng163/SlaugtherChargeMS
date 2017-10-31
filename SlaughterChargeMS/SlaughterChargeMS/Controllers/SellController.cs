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
    public class SellController : BaseController
    {
        readonly ISellService _sellService = new SellService();
        readonly ISellDetailService _sellDetailService = new SellDetailService();

        public ActionResult BatchManage()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public JsonResult QuerySellBatchInfo(SearchSellBatchFilter filter)
        {
            var res = _sellService.QuerySellBatchInfoList(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QuerySellBatchDetailInfo(SearchSellBatchDetailFilter filter)
        {
            var res = _sellService.QuerySellBatchDetaiInfoList(filter);
            return Json(new { total = res.TotalSize, rows = res.Results }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QuerySignleSellBatchInfo(int SellId)
        {
            try
            {
                var data = _sellService.QuerySignleBatchInfo(SellId);
                return Json(new { ret = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ret = false, errorMsg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 得到进货批次的下一个编号
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNextSellBatchNumber()
        {
            try
            {
                int num = _sellService.GetNextSellBatchNumber();
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
        public ContentResult AddSellBatchInfo(DB_Sell model)
        {
            string retMsg = string.Empty;
            try
            {
                model.EnterTime = DateTime.Now;
                model.FK_Entertor = loginAccount.loginUserId;
                _sellService.Add(model, out retMsg);
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
        public ContentResult AddSellBatchDetailInfo(DB_SellDetail model)
        {
            string retMsg = string.Empty;
            try
            {
                model.EnterTime = DateTime.Now;
                _sellDetailService.AddSellDetail(model, out retMsg);
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
        public ContentResult DeleteSellBatchDetailInfo(string IdList)
        {
            string retMsg = string.Empty;
            try
            {
                var list = IdList.Split(',').ToList();
                if (list.Count() == 0)
                    return Content("请选择要删除的信息！");
                var dList = from p in list select Convert.ToInt32(p);
                _sellDetailService.DeleteDetail(dList.ToList(), out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }

        public ContentResult Delete(string IdList)
        {
            string retMsg = string.Empty;
            try
            {
                var list = IdList.Split(',').ToList();
                if (list.Count() == 0)
                    return Content("请选择要删除的信息！");
                var dList = from p in list select Convert.ToInt32(p);
                _sellService.Delete(dList.ToList(), out retMsg);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
            return Content(retMsg);
        }
    }
}
