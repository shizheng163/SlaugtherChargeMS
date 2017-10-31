/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/25 14:34:50
*  文件名：CommonHelper
*  说明： 通用工具类
*───────────────────────────────────
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlaughterChargeMS.Utility
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 微信的时间戳转换为时间
        /// </summary>
        public static DateTime TimeStampConvertToDatetime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}
