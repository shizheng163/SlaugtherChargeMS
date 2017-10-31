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
    public class ArrearsDetail
    {
        public int Id;
        public int UserId;
        public string UserNo;
        public string Name;
        public string Money;
        public string Type;
        public string TransTime;
        public string Remark;
        public string EnterTime;
        public ArrearsDetail() { }
        public ArrearsDetail(DB_UserInfo db_user,DB_ArrearsDetail detail)
        {
            UserId = db_user.Id;
            Id = detail.Id;
            UserNo = db_user.UserNo;
            Name = db_user.Name + '(' + UserNo + ')';
            Type = detail.Type == 0 ? "欠款":"还款";
            Money = string.Format("<label style='color:{0};'>{1}{2}</label>", detail.Type == 0 ? "red" : "#1ab394", detail.Type == 0 ? '+' : '-', detail.Money);
            TransTime = detail.TransTime;
            EnterTime = detail.EnterTime.ToString("F");
            Remark = detail.Remark;
        }

        public ArrearsDetail(DB_UserInfo db_user, DB_CreditDetail detail)
        {
            UserId = db_user.Id;
            Id = detail.Id;
            UserNo = db_user.UserNo;
            Name = db_user.Name + '(' + UserNo + ')';
            Type = detail.Type == 0 ? "欠款" : "还款";
            Money = string.Format("<label style='color:{0};'>{1}{2}</label>", detail.Type == 0 ? "red" : "#1ab394", detail.Type == 0 ? '+' : '-', detail.Money);
            TransTime = detail.TransTime;
            EnterTime = detail.EnterTime.ToString("F");
            Remark = detail.Remark;
        }
    }
}
