/****************************************************************************
 * 单元名称：字典信息类
 * 单元描述： 字典信息类
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel;
using MSIBLL;
using CommonModel.Filter;

namespace MSBLL
{
    public class DictionaryInfoService : BaseService<DB_DictionaryInfo>, IDictionaryInfoService
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = db.DictionaryInfoRepository;
        }

        public PagingResults<DB_DictionaryInfo> GetDictionaryInfos(DictionaryInfoFilter filter)
        {
            var query = LoadEntities(p => p.FK_DictionaryCategory == filter.CategoryCode);

            var count = query.Count();
            query = query.OrderBy(x => x.Code).Skip(filter.PageSize * (filter.PageNo - 1)).Take(filter.PageSize);
            //query = query.Skip(filter.PageSize * (filter.PageNo - 1)).Take(filter.PageSize);
            return new PagingResults<DB_DictionaryInfo>(query) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }


        public bool GetCodeIsExsist(string code)
        {
            int result = LoadEntities(p => p.Code == code).Count();
            return result > 0;
        }

        public bool GetNameIsExsist(string name, string fk_code)
        {
            //当同一个类别名称相同
            int result = LoadEntities(p => p.Name == name && p.FK_DictionaryCategory == fk_code).Count();
            return result > 0;
        }
        /// <summary>
        /// 增加字典信息
        /// </summary>
        /// <param name="model">实体model</param>
        /// <param name="mess">返回错误信息</param>
        /// <returns></returns>
        public bool Add(DB_DictionaryInfo model, out string mess)
        {
            mess = string.Empty;
            try
            {
                if (GetCodeIsExsist(model.Code))
                {
                    mess = "该编码信息已经存在!";
                    return false;
                }
                if (GetNameIsExsist(model.Name, model.FK_DictionaryCategory))
                {
                    mess = "该名称信息已经存在!";
                    return false;
                }
                var result = AddEntities(model);
                return true;
            }
            catch (Exception ex)
            {

                mess = ex.Message;
                return false;
            }
        }

        public bool GetCodeNameIsExsist(string code, string name, string fk_code)
        {
            //当同一个类别的code不同，名称相同
            int resule = LoadEntities(p => p.Name == name && p.Code != code && p.FK_DictionaryCategory == fk_code).Count();
            return resule > 0;
        }

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="model">实体model</param>
        /// <param name="mess">返回错误信息</param>
        /// <returns></returns>
        public bool Update(DB_DictionaryInfo model, out string mess)
        {
            mess = string.Empty;
            try
            {
                if (GetCodeNameIsExsist(model.Code, model.Name, model.FK_DictionaryCategory))
                {
                    mess = "该名称信息已经存在!";
                    return false;
                }
                return UpdateEntities(model);
            }
            catch (Exception ex)
            {
                mess = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除字典信息
        /// </summary>
        /// <param name="code">编号</param>
        /// <param name="mess">返回错误信息</param>
        /// <returns></returns>
        public bool Delete(string code, out string mess)
        {
            mess = string.Empty;
            try
            {
                var dic = LoadEntities(p => p.Code == code).FirstOrDefault();
                if (dic != null)
                {
                    return DeleteEntities(dic);
                }
                else
                {
                    mess = string.Format("该信息[{0}]已不存在!", code);
                    return false;
                }
            }
            catch (Exception objErr)
            {
                mess = "删除字典项失败，该字典项可能正在使用中！";
                string err = "Error Caught in Application_Error event\n" +
                                    " Error in：" + "类名:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "\n" + "方法名:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n" +
                                    "\n Error Message：" + objErr.Message.ToString() +
                                    "\n Stack Trace：" + objErr.StackTrace.ToString();
                //将捕获的错误写入应用程序日志中
                return false;
            }
        }

        /// <summary>
        /// 返回某类字典信息表中最大的编号
        /// </summary>
        /// <param name="fk_code">字典类型</param>
        /// <returns>最大的编号</returns>
        public string GetMaxCode(string fk_code)
        {
            
            var models = LoadEntities(p => p.FK_DictionaryCategory == fk_code);
            if (0 == models.Count())
            {
                return fk_code + "00";
            }
            else
            {
                return models.Last().Code;
            }
        }


        public List<DB_DictionaryInfo> GetALLByFKDictionaryCategory(string FKDictionaryCategory)
        {
            var models = LoadEntities(p => p.FK_DictionaryCategory == FKDictionaryCategory);
            if (0 == models.Count())
            {
                return null;
            }
            else
            {
                return models.ToList();
            }
        }

        /// <summary>
        /// 获得该类型实体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DB_DictionaryInfo GetModel(string code)
        {
            try
            {
                var user = LoadEntities(p => p.Code == code).FirstOrDefault();
                return user;
            }
            catch
            {
                return null;
            }
        }



        public DB_DictionaryInfo GetModel(string name, string ParentCategoryCode)
        {
            try
            {
                var user = LoadEntities(p => p.Name == name && p.FK_DictionaryCategory == ParentCategoryCode).FirstOrDefault();
                return user;
            }
            catch {
                return null;
            }
            
        }


        public string GetDictionaryName(string code)
        {
            var model = LoadEntities(p => p.Code == code).FirstOrDefault();
            if (model == null)
                return "";
            return model.Name;
        }


        public string GetDicNameCollect(string codeCollect)
        {
            var codeArray = codeCollect.Split(',');
            string result = "";
            foreach (var code in codeArray)
            {
                result += GetDictionaryName(code) + ",";
            }
            if (codeArray.Length != 0)
                result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}
