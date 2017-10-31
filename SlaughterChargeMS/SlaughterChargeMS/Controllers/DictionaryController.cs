/****************************************************************************
 * 单元名称：字典设置控制器类
 * 单元描述：字典设置控制器类
 * 作者：hzh
 * 创建日期：2016-06-01 
 * 最后修改：（请最后修改的人填写）
 * 修改日期：XXXX-XX-XX
 * 版本号：Ver 1(每次修改 加 1)
 * (C) 2016 
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSIBLL;
using MSBLL;
using CommonModel;
using CommonModel.Filter;
namespace SlaughterChargeMS.Controllers
{
    public class DictionaryController : BaseController
    {
        IDictionaryCategoryService _dictCategoryservice = new DictionaryCategoryService();
        IDictionaryInfoService _dictInfoservice = new DictionaryInfoService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获得字典类别
        /// </summary>
        /// <returns>字典类别</returns>
        public JsonResult GetDicCategory()
        {
            DictionaryCategoryFilter filter = new DictionaryCategoryFilter();
            filter.CanModify = true;

            filter.page=1;
            filter.rows=20;

            var dicList = _dictCategoryservice.GetDictionaryCategorys(filter);
            var result = from d in dicList.Results
                         select new
                         {
                             Code = d.Code,
                             Name = d.Name
                         };

            return Json(new { total = dicList.TotalSize, rows = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得字典信息
        /// </summary>
        /// <param name="code">字典类别编号</param>
        /// <returns></returns>
        public JsonResult GetDicCategoryInfo(string code)
        {
            DictionaryInfoFilter filter = new DictionaryInfoFilter();
            filter.CategoryCode = code;

            
            filter.page=Convert.ToInt32(Request["page"]);
            filter.rows=Convert.ToInt32(Request["rows"]);
            //filter.page=1;
            //filter.rows=20;
            var dicList = _dictInfoservice.GetDictionaryInfos(filter);
            var result = from d in dicList.Results
                         select new
                         {
                             Code = d.Code,
                             Name = d.Name,
                             Remark = d.Remark
                         };

            return Json(new { total = dicList.TotalSize, rows = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 增加字典信息
        /// </summary>
        /// <param name="model">字典信息</param>
        /// <returns>增加结果</returns>
        [HttpPost]
        public ActionResult AddDicCategoryInfo(DB_DictionaryInfo model)
        {
            string retMsg = "";
            bool retFlag = false;

            try
            {
                //获取操作员信息
                model.OperatorCode = loginAccount.loginName;
                model.OperatorName = loginAccount.name;

                //获取当前时间
                model.OperatorTime = DateTime.Now;

                retFlag = _dictInfoservice.Add(model, out retMsg);
            }
            catch (Exception e)
            {
                retMsg = string.Format("发生错误:{0}", e.Message);
                //Util.CLogServiceHandler.ErrorLogAdd(retMsg);
            }
            return Content(retMsg);
        }

        /// <summary>
        /// 删除字典信息
        /// </summary>
        /// <param name="ids">所有要删除的字典信息的Code</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult removeDicCategoryInfo(string ids)
        {
            string[] s = ids.Split(new char[] { ',' });

            string retMsg = "";
            try
            {
                foreach (string str in s)
                {
                    _dictInfoservice.Delete(str, out retMsg);
                }
                //retMsg = code;
                //ret = _bedTypeService.Delete();
            }
            catch (Exception e)
            {
                retMsg = string.Format("发生错误:{0}", e.Message);
                //Util.CLogServiceHandler.ErrorLogAdd(retMsg);
            }
            return Content(retMsg);
        }

        /// <summary>
        /// 修改字典信息
        /// </summary>
        /// <param name="model">修改后的字典信息</param>
        /// <returns>修改结果</returns>
        [HttpPost]
        public ActionResult UpdateDicCategoryInfo(DB_DictionaryInfo model)
        {
            string retMsg = "";
            bool retFlag = false;

            try
            {
                //获取操作员信息
                model.OperatorCode = loginAccount.loginName;
                model.OperatorName = loginAccount.name;
                //获取当前时间
                model.OperatorTime = DateTime.Now;

                retFlag = _dictInfoservice.Update(model, out retMsg);
            }
            catch (Exception e)
            {
                retMsg = string.Format("发生错误:{0}", e.Message);
                //Util.CLogServiceHandler.ErrorLogAdd(retMsg);
            }
            return Content(retMsg);
        }

        /// <summary>
        /// 生成待添加的字典信息的编号
        /// </summary>
        /// <param name="FKCode">字典类别编号</param>
        /// <returns>生成的编号</returns>
        public JsonResult GetGeneraoteCode(string FKCode)
        {
            var maxCode = _dictInfoservice.GetMaxCode(FKCode);
            var newCode = "01";

            var t = maxCode.Substring(FKCode.Length);
            var tmp = Convert.ToInt32(maxCode.Substring(FKCode.Length));
            if (0 == tmp)
            {
                newCode = "01";
            }
            else
                if (tmp < 9)
                {
                    newCode = "0" + (tmp + 1);
                }
                else
                    {
                        newCode = (tmp + 1).ToString();
                    }
            return Json(new { Code = newCode }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据字典分类的名称获得所有字典数据JSON数据
        /// </summary>
        /// <param name="DictionaryCategoryName">字典分类名称</param>
        /// <returns></returns>
        public JsonResult GetComboBoxValue(string name)
        {
            string code = _dictCategoryservice.GetCodeByName(name);
            if (null != code)
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
    }
}