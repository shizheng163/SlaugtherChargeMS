using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 前端详细批次信息
    /// </summary>
    public class SellBatchDetailInfo
    {
        public int Id;
        /// <summary>
        /// 所在批次
        /// </summary>
        public int BatchNumber;
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TransTime;
        /// <summary>
        /// 交易对象
        /// </summary>
        public string TranscationObject;
        /// <summary>
        /// 交易原因
        /// </summary>
        public string TransReason;
        /// <summary>
        /// 交易货物数量
        /// </summary>
        public string TransNum;
        /// <summary>
        /// 交易后剩余数量
        /// </summary>
        public int RemainderNum;
        /// <summary>
        /// 交易金额
        /// </summary>
        public string TrnasCost;
        /// <summary>
        /// 收支类型
        /// </summary>
        public string SpendType;
        /// <summary>
        /// 录入时间
        /// </summary>
        public string EnterTime;
    }
}
