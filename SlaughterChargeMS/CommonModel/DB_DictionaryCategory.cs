//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class DB_DictionaryCategory
    {
        public DB_DictionaryCategory()
        {
            this.DB_DictionaryInfo = new HashSet<DB_DictionaryInfo>();
        }
    
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> OperatorTime { get; set; }
        public string OperatorCode { get; set; }
        public string OperatorName { get; set; }
        public bool CanModify { get; set; }
    
        public virtual ICollection<DB_DictionaryInfo> DB_DictionaryInfo { get; set; }
    }
}