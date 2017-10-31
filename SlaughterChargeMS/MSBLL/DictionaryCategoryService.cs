/****************************************************************************
 * 单元名称：字典信息类别类
 * 单元描述： 字典信息类
 * 作者：沈涛
 * 创建日期：2016-05-26 
 * 最后修改：（请最后修改的人填写）
 * 修改日期：XXXX-XX-XX
 * 版本号：Ver 1(每次修改 加 1)
 * (C) 2016 新友
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
    public class DictionaryCategoryService:BaseService<DB_DictionaryCategory>,IDictionaryCategoryService
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = db.DictionaryCategoryRepository;
        }

        public PagingResults<DB_DictionaryCategory> GetDictionaryCategorys(DictionaryCategoryFilter filter)
        {
            var query = LoadEntities(p=>(p.CanModify==filter.CanModify));
            return new PagingResults<DB_DictionaryCategory>(query) { PageSize = filter.PageSize, TotalSize = query.Count(), CurrentPageNo = filter.PageNo };
        }

        public string GetCodeByName(string name)
        {
            var query = LoadEntities(p=>(p.Name==name));
            if(query.Count()==0)
                return null;
            else
            {
                return query.First().Code;
            }
        }

        public Dictionary<string, string> GetDictByCategorys(string Code)
        {
            var query = db.DictionaryInfoRepository.LoadEntities(p => p.FK_DictionaryCategory == Code);
            var dict = new Dictionary<string, string>();
            foreach (var info in query)
            {
                dict[info.Name] = info.Code;
            }
            return dict;
        }


        public Dictionary<string, string> GetDictByCategorysCode_Name(string Code)
        {
            var query = db.DictionaryInfoRepository.LoadEntities(p => p.FK_DictionaryCategory == Code);
            var dict = new Dictionary<string, string>();
            foreach (var info in query)
            {
                dict[info.Code] = info.Name;
            }
            return dict;
        }
    }
}
