using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel;
using CommonModel.Filter;

namespace MSIBLL
{
    public partial interface IDictionaryCategoryService:IBaseService<DB_DictionaryCategory>
    {
        /// <summary>
        /// 根据检索条件分页显示
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        PagingResults<DB_DictionaryCategory> GetDictionaryCategorys(DictionaryCategoryFilter filter);

        /// <summary>
        /// 根据名称获得Code
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        string GetCodeByName(string name);
        /// <summary>
        /// 通过大类编码获取字典详情
        /// </summary>
        /// <param name="Code"></param>
        /// <returns>Code-Name</returns>
        Dictionary<string, string> GetDictByCategorys(string Code);

        /// <summary>
        /// 通过大类编码获取字典详情
        /// </summary>
        /// <param name="Code"></param>
        /// <returns>Code-Name</returns>
        Dictionary<string, string> GetDictByCategorysCode_Name(string Code);
    }
}
