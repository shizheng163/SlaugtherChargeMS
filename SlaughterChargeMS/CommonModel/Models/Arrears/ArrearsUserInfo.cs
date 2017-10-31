using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 按人员显示欠费信息
    /// </summary>
    public class ArrearsUserInfo
    {
        public int Id;
        public string UserNo;
        public string Name;
        public string Company;
        public string Phone;
        public ArrearsUserInfo() { }
        public ArrearsUserInfo(DB_UserInfo db_user)
        {
            Id = db_user.Id;
            UserNo = db_user.UserNo;
            Name = db_user.Name;
            Company = db_user.Company;
            Phone = db_user.Phone;
        }
        /// <summary>
        /// 欠款金额 加入Label颜色控制
        /// </summary>
        public string ArrearsNum;
    }
}
