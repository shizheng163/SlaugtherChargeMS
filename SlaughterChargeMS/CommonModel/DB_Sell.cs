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
    
    public partial class DB_Sell
    {
        public DB_Sell()
        {
            this.DB_SellDetail = new HashSet<DB_SellDetail>();
        }
    
        public int Id { get; set; }
        public int BatchNumber { get; set; }
        public string QuantityDate { get; set; }
        public string TransactionObject { get; set; }
        public string FK_SellCategory { get; set; }
        public int QuantityNum { get; set; }
        public int RemainderNum { get; set; }
        public double QuantitySpend { get; set; }
        public double OtherSpend { get; set; }
        public double CurIncome { get; set; }
        public double CurProfit { get; set; }
        public System.DateTime EnterTime { get; set; }
        public int FK_Entertor { get; set; }
    
        public virtual DB_DictionaryInfo DB_DictionaryInfo { get; set; }
        public virtual DB_Manager DB_Manager { get; set; }
        public virtual ICollection<DB_SellDetail> DB_SellDetail { get; set; }
    }
}
