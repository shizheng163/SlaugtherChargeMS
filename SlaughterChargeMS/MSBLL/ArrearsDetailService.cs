/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 14:52:17
*  文件名：ArrearsDetailService
*  说明： 欠款管理的业务层实现
*───────────────────────────────────
*  V0.01 2017/7/21 14:52:17 史正 初版
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSIBLL;
using CommonModel;
using CommonModel.Filter;
using CommonModel.Models;
using MSDAL;
namespace MSBLL
{
    public class ArrearsDetailService : BaseService<DB_ArrearsDetail>, IArrearsDetailService
    {
        public override void SetCurrentRepository()
        {
            CurrentRepository = db.ArrearsDetailRepository;
        }

        public void Add(DB_ArrearsDetail model, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                if (model.Type == 1)
                {
                    var arrearsMoney = GetUserArrearsMoney(model.FK_UserInfo);
                    if (model.Money > arrearsMoney)
                    {
                        retMsg = string.Format("当前还款金额为:{0},当前欠款金额为{1},还款金额超出欠款金额,无法还款。", model.Money, arrearsMoney);
                        return;
                    }
                }
                model.EnterTime = DateTime.Now;
                AddEntities(model);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void Update(DB_ArrearsDetail model, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                var oldModel = GetModel(model.Id);
                if (oldModel.Type != model.Type)
                {
                    retMsg = "类型无法改变。";
                }
                #region MyRegion
                //if (model.Type == 1)
                //{
                //    var arrearsMoney = GetUserArrearsMoney(model.FK_UserInfo);
                //    if (model.Money > arrearsMoney)
                //    {
                //        retMsg = string.Format("当前还款金额为:{0},当前欠款金额为{1},还款金额超出欠款金额,无法还款。", model.Money, arrearsMoney);
                //        return;
                //    }
                //}
                #endregion
                model.EnterTime = DateTime.Now;
                AddEntities(model);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public DB_ArrearsDetail GetModel(int Id)
        {
            return LoadEntities(p => p.Id == Id).FirstOrDefault();
        }

        public int GetUserArrearsMoney(int UserId)
        {
            try {
                var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
                var query = efdb.DB_ArrearsDetail.Where(p => p.FK_UserInfo == UserId).AsQueryable();
                var group = query.GroupBy(p => p.Type).Select(k => new { Type = k.Key, Value = k.Sum(p => p.Money) }).ToDictionary(p=>p.Type);
                var temp1 = group.ContainsKey(0) ? group[0].Value : 0;
                var temp2 = group.ContainsKey(1) ? group[1].Value : 0;
                return temp1 - temp2;
            } catch
            {
                return 0;
            }
            
        }

        public void Delete(List<int> list, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_ArrearsDetail> deleteList = new List<DB_ArrearsDetail>();
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

        public PagingResults<ArrearsUserInfo> QueryUserInfoArrearsWithPager(SearchArrearsUserInfoFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var query = efdb.DB_ArrearsDetail.GroupBy(p => p.FK_UserInfo);
            var list = new List<ArrearsUserInfo>();
            foreach (var info in query)
            {
                var db_user = db.UserInfoRepository.LoadEntities(p => p.Id == info.Key).FirstOrDefault();
                var node = new ArrearsUserInfo(db_user);
                int num = GetUserArrearsMoney(info.Key);
                node.ArrearsNum = string.Format("{0}<label style='color:red;'>{1}</label>", num >= 0 ? "欠款" : "数据错误,还款金额大于欠款金额。", num);
                list.Add(node);
            }
            int count = list.Count();
            var res  = list.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            return new PagingResults<ArrearsUserInfo>(res) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }

        public PagingResults<ArrearsDetail> QueryArrearsDetailWithPager(SearchArrearsDetailFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            IQueryable<DB_ArrearsDetail> query = efdb.DB_ArrearsDetail.Include("DB_UserInfo").OrderBy(p=>p.TransTime);
            if (!string.IsNullOrEmpty(filter.Range))
            {
                query = query.Where(p => p.TransTime.StartsWith(filter.Range));
            }
            if (filter.SearchType != 2)
                query = query.Where(p => p.Type == filter.SearchType);
            if (filter.FK_UserInfo != 0)
                query = query.Where(p => p.FK_UserInfo == filter.FK_UserInfo);

            int count = query.Count();
            query = query.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            var list = new List<ArrearsDetail>();
            foreach (var info in query)
            {
                var node = new ArrearsDetail(info.DB_UserInfo, info);
                list.Add(node);
            }
            return new PagingResults<ArrearsDetail>(list) { TotalSize = count, CurrentPageNo = filter.PageNo, PageSize = filter.PageSize };
        }
    }
}
