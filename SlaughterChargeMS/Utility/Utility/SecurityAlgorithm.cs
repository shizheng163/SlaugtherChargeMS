using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using SlaughterChargeMS.Config;
namespace SlaughterChargeMS.Utility
{
    /// <summary>
    /// 安全加密算法实现类
    /// </summary>
    public class SecurityAlgorithm
    {
        #region SHA1
        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string SHA1Security(string password)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            string sha1 = "";
            for (int i = 0; i < data.Length; i++)
            {
                sha1 += data[i].ToString("x2").ToUpperInvariant();
            }
            return sha1;
        }
        #endregion

        #region DES对称式加密解密算法
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="code">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DesEncrypt(string code, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(code);
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="code">解密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesDecrypt(string code, string key)
        {
            byte[] inputByteArray = Convert.FromBase64String(code);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(key);
                des.IV = ASCIIEncoding.ASCII.GetBytes(key);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }


        #endregion

        #region PBKDF2
        public class PBKDF2
        {
            /// <summary>
            /// 加密或验证时通过明文与盐获得密码，16进制
            /// </summary>
            /// <param name="password">输入的密码明文</param>
            /// <param name="salt">盐</param>
            /// <param name="iterations">迭代次数</param>
            /// <param name="length">输出密码字节数，长度为length*2</param>
            /// <returns></returns>
            public static string GetPassword(string password, string salt, int length)
            {
                byte[] saltb = Encoding.Default.GetBytes(salt);
                var kd = new Rfc2898DeriveBytes(password, saltb,BackConfig.IteartorTimes_PBKDF2);
                string result = BitConverter.ToString(kd.GetBytes(length));
                result = result.Replace("-", "");
                return result;
            }
            /// <summary>
            /// 随机生成盐,返回结果为输入长度的2倍
            /// </summary>
            /// <param name="length">长度</param>
            /// <returns></returns>
            private static string GetSalt(int length)
            {
                byte[] salt = new byte[length];
                var rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
                string result = BitConverter.ToString(salt);
                result = result.Replace("-", "");
                return result;
            }

            /// <summary>
            /// 创建64位PBKDF2加密的密码(14为盐,50位为加密后的密码)
            /// </summary>
            /// <param name="pwd">输入的密码明文</param>
            /// <returns></returns>
            public static string Create64PBKDF2Pwd(string pwd)
            {
                string salt = GetSalt(BackConfig.SaltLength);
                string securePassword = GetPassword(pwd, salt, BackConfig.EncryptPwdLength);
                return salt + securePassword;
            }
        }
        #endregion
    }
}
