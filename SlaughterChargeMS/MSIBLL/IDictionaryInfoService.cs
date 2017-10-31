using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel;
using CommonModel.Filter;

namespace MSIBLL
{
    public partial interface IDictionaryInfoService:IBaseService<DB_DictionaryInfo>
    {
        /// <summary>
        /// 根据检索条件分页显示
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        PagingResults<DB_DictionaryInfo> GetDictionaryInfos(DictionaryInfoFilter filter);

        /// <summary>
        /// 增加字典信息
        /// </summary>
        /// <param name="model">实体model</param>
        /// <param name="mess">返回错误信息</param>
        /// <returns></returns>
        bool Add(DB_DictionaryInfo model, out string mess);

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="model">实体model</param>
        /// <param name="mess">返回错误信息</param>
        /// <returns></returns>
        bool Update(DB_DictionaryInfo model, out string mess);

        /// <summary>
        /// 删除字典信息
        /// </summary>
        /// <param name="code">编号</param>
        /// <param name="mess">返回错误信息</param>
        /// <returns></returns>
        bool Delete(string code, out string mess);

        /// <summary>
        /// 返回某类字典信息表中最大的编号
        /// </summary>
        /// <param name="fk_code">字典类型</param>
        /// <returns>最大的编号</returns>
        string GetMaxCode(string fk_code);

       /// <summary>
       /// 根据字典分类获得字典信息
       /// </summary>
       /// <param name="FKDictionaryCategoryCode">字典分类Code</param>
       /// <returns></returns>
        List<DB_DictionaryInfo> GetALLByFKDictionaryCategory(string FKDictionaryCategoryCode);

        /// <summary>
        /// 获得该类型实体
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DB_DictionaryInfo GetModel(string code);
        /// <summary>
        /// 通过字典名称，和父大类Code获取
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ParentCategoryCode"></param>
        /// <returns></returns>
        DB_DictionaryInfo GetModel(string name,string ParentCategoryCode);
        /// <summary>
        /// 通过Code获取字典名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetDictionaryName(string code);

        /// <summary>
        /// 传入字典Code编码集合,格式按照1,2,3,4分格
        /// </summary>
        /// <param name="codeCollect"></param>
        /// <returns>返回名称的集合 1,2,3,4</returns>
        string GetDicNameCollect(string codeCollect);
    }
}
