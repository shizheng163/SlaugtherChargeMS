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
    
    public partial class DB_ArrearsDetail
    {
        public int Id { get; set; }
        public short Type { get; set; }
        public int FK_UserInfo { get; set; }
        public int Money { get; set; }
        public string TransTime { get; set; }
        public System.DateTime EnterTime { get; set; }
        public string Remark { get; set; }
    
        public virtual DB_UserInfo DB_UserInfo { get; set; }
    }
}
