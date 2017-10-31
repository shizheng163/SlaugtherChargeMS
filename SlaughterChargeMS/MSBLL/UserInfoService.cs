/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/24 10:09:02
*  文件名：UserInfoService
*  说明： 微信用户扩展信息业务层接口实现
*───────────────────────────────────
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel;
using MSIBLL;
using CommonModel.Filter;
using CommonModel.Models;
using MSDAL;
using SlaughterChargeMS.Utility;
using System.Transactions;
using SlaughterChargeMS.Config;
namespace MSBLL
{
    public class UserInfoService : BaseService<DB_UserInfo>, IUserInfoService
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = db.UserInfoRepository;
        }

        public void Add(DB_UserInfo user, out string mess)
        {
            mess = string.Empty;
            try
            {
                user.UserNo = GetNextUserNo();
                if (!string.IsNullOrEmpty(user.Phone) && CheckPhone(0, user.Phone))
                    throw new Exception("此电话号码已经存在:" + user.Phone);
                if (string.IsNullOrEmpty(user.Name))
                    throw new Exception("姓名不允许为空。");
                //if (!string.IsNullOrWhiteSpace(user.Company)&&LoadEntities(p => p.Name == user.Name && p.Company == user.Company).Any())
                //    throw new Exception("该公司下已经有此姓名的员工存在!");
                AddEntities(user);
            }
            catch (Exception ex)
            {
                mess = ex.Message;
            }
        }

        private bool CheckPhone(int id, string Phone)
        {
            return LoadEntities(p => p.Id != id && p.Phone == Phone).Count() > 0;
        }
        public void Update(DB_UserInfo user, out string mess)
        {
            mess = string.Empty;
            try
            {
                if (CheckPhone(user.Id, user.Phone))
                    throw new Exception("此电话号码已经存在,或与他人的学工号相同");
                if (string.IsNullOrEmpty(user.Name))
                    throw new Exception("姓名不允许为空。");
                if (!string.IsNullOrWhiteSpace(user.Company) && LoadEntities(p =>user.Id!=p.Id && p.Name == user.Name && p.Company == user.Company).Any())
                    throw new Exception("该公司下已经有此姓名的员工存在!");
                UpdateEntities(user);
            }
            catch (Exception ex)
            {
                mess = ex.Message;
            }
        }

        public DB_UserInfo GetModel(string UserNo)
        {
            return LoadEntities(p => p.UserNo == UserNo).FirstOrDefault();
        }

        public DB_UserInfo GetModel(int id)
        {
            return LoadEntities(p => p.Id == id).FirstOrDefault();
        }

        public string GetNextUserNo()
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var local = efdb.DB_UserInfo.Local;
            if (local.Count() == 0)//缓冲中没有
            {
                var data = LoadEntities(p => true).LastOrDefault();
                if (data == null)//数据库中无数据时
                {
                    return "000001";
                }
                else
                {
                    return (Convert.ToInt32(data.UserNo) + 1).ToString("000000");
                }
            }
            else
            {
                var data = local.OrderBy(p => p.UserNo).LastOrDefault();
                return (Convert.ToInt32(data.UserNo) + 1).ToString("000000");
            }
        }

        public PagingResults<UserShowInfo> QueryUserInfoWithPager(SearchUserInfoFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var query = efdb.DB_UserInfo.OrderBy(p=>p.UserNo).AsQueryable();
            int count = query.Count();
            query = query.Skip(filter.PageSize * (filter.PageNo - 1)).Take(filter.PageSize);
            var res = from db_user in query
                      select new UserShowInfo()
                      {
                          Id = db_user.Id,
                          UserNo = db_user.UserNo,
                          Name = db_user.Name,
                          Company = db_user.Company,
                          Phone = db_user.Phone,
                      };
            return new PagingResults<UserShowInfo>(res) { PageSize = filter.PageSize, CurrentPageNo = filter.PageNo, TotalSize = count };
        }

        public void Delete(List<int> list, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_UserInfo> deleteList = new List<DB_UserInfo>();
                foreach (var Id in list)
                {
                    var info = LoadEntities(p => p.Id == Id).FirstOrDefault();
                    deleteList.Add(info);
                }
                DeleteEntities(deleteList);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }
    }
}
