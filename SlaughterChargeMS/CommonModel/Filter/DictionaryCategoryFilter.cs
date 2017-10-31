using System;

namespace CommonModel.Filter
{
    /// <summary>
    /// 过滤的条件
    /// </summary>
    [Serializable]
    public class DictionaryCategoryFilter : PagingFilter
    {
        /// <summary>
        /// 是否可以修改
        /// </summary>
        public bool CanModify { get; set; }
    }


    [Serializable]
    public class DictionaryInfoFilter : PagingFilter
    {
        /// <summary>
        /// CategoryCode
        /// </summary>
        public string CategoryCode { get; set; }
    }
}
