using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 用户管理界面前端显示的用户信息
    /// </summary>
    public class UserShowInfo
    {
        public int Id;
        public string UserNo;
        public string Name;
        public string Company;
        public string Phone;
        public UserShowInfo() { }
        public UserShowInfo(DB_UserInfo db_user)
        {
            Id = db_user.Id;
            UserNo = db_user.UserNo;
            Name = db_user.Name;
            Company = db_user.Company;
            Phone = db_user.Phone;
        }
    }
}
