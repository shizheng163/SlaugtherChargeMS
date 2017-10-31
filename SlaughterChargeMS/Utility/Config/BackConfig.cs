/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 16:12:20
*  文件名：BackConfig
*  说明： 
*───────────────────────────────────
*  V0.01 2017/7/21 16:12:20 史正 初版
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlaughterChargeMS.Config
{
    /// <summary>
    /// 后台开发所用到的配置信息在这里
    /// </summary>
    public class BackConfig
    {
        #region PBKDF2
        /// <summary>
        /// 生成PBKDF2密码 盐的长度（实际长度为此值的两倍）
        /// EncryptPwdLength+SaltLength 应满足 小于32的条件 （数据库限制）
        /// </summary>
        public const int SaltLength = 7;

        /// <summary>
        /// 生成真正的PBKDF2密码的长度 （实际长度为此值两倍）
        /// EncryptPwdLength+SaltLength 应满足 小于32的条件 （数据库限制）
        /// </summary>
        public const int EncryptPwdLength = 25;

        /// <summary>
        /// 生成PBKDF2密码时的迭代次数
        /// </summary>
        public const int IteartorTimes_PBKDF2 = 3;
        #endregion
    }
}
