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
    
    public partial class DB_Manager
    {
        public DB_Manager()
        {
            this.DB_Sell = new HashSet<DB_Sell>();
        }
    
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Menus { get; set; }
        public string OperatorName { get; set; }
        public string OperatorTime { get; set; }
    
        public virtual ICollection<DB_Sell> DB_Sell { get; set; }
    }
}
