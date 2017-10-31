/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017年8月16日22:46:06
*  文件名：ButcherProcessService
*  说明： 屠宰加工管理业务层实现
*───────────────────────────────────
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
    public class ButcherProcessService : BaseService<DB_ButcherProcess>, IButcherProcessService
    {


        public override void SetCurrentRepository()
        {
            CurrentRepository = db.ButcherProcessRepository;
        }

        public void Add(DB_ButcherProcess model, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                var date = model.TransTime.Split('-');
                var dateTime = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]));
                if (dateTime > DateTime.Now.Date)
                {
                    retMsg = "交易日期不可大于今天";
                    return;
                }
                model.EnterTime = DateTime.Now;
                AddEntities(model);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public PagingResults<ButcherDetail> QueryButcherDetailWithPager(SearchButcherDetailFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            IQueryable<DB_ButcherProcess> query = null;
            if(string.IsNullOrEmpty(filter.Range))
                query = efdb.DB_ButcherProcess.Include("DB_DictionaryInfo").OrderByDescending(p => p.TransTime).AsQueryable();
            else
                query = efdb.DB_ButcherProcess.Include("DB_DictionaryInfo").Where(p=>p.TransTime.StartsWith(filter.Range)).OrderByDescending(p => p.TransTime).AsQueryable();
            if (filter.SearchType != 2)
            {
                if (filter.ReasonCode != "0")
                    query = query.Where(p => p.Type == filter.SearchType && p.FK_Reason == filter.ReasonCode);
                else
                    query = query.Where(p => p.Type == filter.SearchType);
            }
            int count = query.Count();
            query = query.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            var list = new List<ButcherDetail>();
            foreach (var info in query)
            {
                var node = new ButcherDetail()
                {
                    Id = info.Id,
                    Type = info.Type == 0 ? "收入" : "支出",
                    Reason = info.DB_DictionaryInfo.Name,
                    TranscationObject = info.TransactionObject,
                    TransTime = info.TransTime,
                    EnterTime = info.EnterTime.ToString("F"),
                    UpdateNum = info.UpdateNum,
                    SingleAmount = info.SingleAmount,
                    TotalSpend = (info.Type == 1?"-":"+")+info.SingleAmount*info.UpdateNum
                };
                node.TotalSpend = string.Format("<label style='color:{0};'>{1}</label>", (info.Type == 1 ? "#1ab394" : "red"),node.TotalSpend);
                list.Add(node);
            }
            return new PagingResults<ButcherDetail>(list) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }

        public void DeleteDetail(List<int> list, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_ButcherProcess> deleteList = new List<DB_ButcherProcess>();
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

        public PagingResults<ButcherStatis> QueryButcherStatisWithPagerWithPager(SearchButStatisFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            IQueryable<DB_ButcherProcess> query = null;
            if (string.IsNullOrEmpty(filter.Range))
            {
                query = efdb.DB_ButcherProcess.AsQueryable();
            }
            else
            {
                query = efdb.DB_ButcherProcess.Where(p => p.TransTime.StartsWith(filter.Range)).AsQueryable();
            }
            IQueryable<IGrouping<string, DB_ButcherProcess>> groupModel = null;
            if (filter.Type == 0)
            {
                groupModel = query.GroupBy(p => p.TransTime);
            }
            else if (filter.Type == 1)
            {
                groupModel = query.GroupBy(p => p.TransTime.Substring(0,7));
            }
            else
                groupModel = query.GroupBy(p => p.TransTime.Substring(0,4));

            var list = new List<ButcherStatis>();
            int index = 0;
            foreach (var info in groupModel)
            {
                var node = new ButcherStatis()
                {
                    Index = ++index,
                    Range = info.Key
                };
                var collect = info.GroupBy(p => p.Type).Select(k => new { Type = k.Key, Value = k.Sum(p => p.UpdateNum * p.SingleAmount) });

                var incomeCollect = collect.Where(p => p.Type == 0);
                var payCollect = collect.Where(p => p.Type == 1);
                int income = incomeCollect.FirstOrDefault() == null? 0:incomeCollect.FirstOrDefault().Value;
                int pay = payCollect.FirstOrDefault() == null ? 0 : payCollect.FirstOrDefault().Value;
                node.IncomeMoney = "+" + income;
                node.PayMoney = "-" + pay;
                node.ProfitMoney = "" + (income - pay);
                node.ProfitMoney = string.Format("<label style='color:{0};'>{1}</label>", node.ProfitMoney.StartsWith("-") ? "#1ab394" : "red", node.ProfitMoney.StartsWith("-")? node.ProfitMoney:"+"+ node.ProfitMoney);
                list.Add(node);
            }
            int count = query.Count();
            var res = list.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            return new PagingResults<ButcherStatis>(res) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }

        public PagingResults<ButcherStatisView> QueryButcherStatisViewWithPagerWithPager(SearchButcherStatisViewFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            IQueryable<DB_ButcherProcess> query = efdb.DB_ButcherProcess.Include("DB_DictionaryInfo").Where(p=>p.TransTime.StartsWith(filter.Range));
            IQueryable<IGrouping<string, DB_ButcherProcess>> groupModel = query.GroupBy(p => p.FK_Reason);
            var collect = groupModel.Select(p => new { key = p.Key, UpdateNum = p.Sum(k => k.UpdateNum), TotalMoney = p.Sum(k => k.UpdateNum * k.SingleAmount),Name = p.FirstOrDefault().DB_DictionaryInfo.Name });

            var list = new List<ButcherStatisView>();
            foreach (var info in collect)
            {
                var node = new ButcherStatisView()
                {
                    ReasonCode = info.key,
                    ReasonName = info.Name,
                    UpdateNum = info.key.StartsWith("03")?info.UpdateNum:0,
                    TotalMoney = (info.key.StartsWith("03")?"+":"-")+info.TotalMoney
                };
                node.TotalMoney = string.Format("<label style='color:{0};'>{1}</label>", node.TotalMoney.StartsWith("-") ? "#1ab394" : "red",node.TotalMoney);
                list.Add(node);
            }
            int count = query.Count();
            var res = list.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            return new PagingResults<ButcherStatisView>(res) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }
    }
}
