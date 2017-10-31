/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 14:52:17
*  文件名：MenuService
*  说明： 
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
    public class SellService : BaseService<DB_Sell>, ISellService
    {

        public override void SetCurrentRepository()
        {
            CurrentRepository = db.SellRepository;
        }

        public PagingResults<SellBatchInfo> QuerySellBatchInfoList(SearchSellBatchFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var query = efdb.DB_Sell.Include("DB_DictionaryInfo").OrderBy(p=>p.BatchNumber).AsQueryable();
            int count = query.Count();
            query = query.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            var list = new List<SellBatchInfo>();
            foreach (var info in query)
            {
                var node = new SellBatchInfo()
                {
                    Id = info.Id,
                    BatchNumber = info.BatchNumber,
                    QuantityDate = info.QuantityDate,
                    TranscationObject = info.TransactionObject,
                    SellCategory = info.DB_DictionaryInfo.Name,
                    QuantityNum = info.QuantityNum,
                    RemainderNum = info.RemainderNum,
                    QuantitySpend = info.QuantitySpend,
                    OtherSpend = info.OtherSpend,
                    CurIncome = info.CurIncome,
                    CurProfit = info.CurProfit,
                    EnterTime = info.EnterTime.ToString("F"),
                    Operator = info.DB_Manager.Name
                };
                list.Add(node);
            }
            return new PagingResults<SellBatchInfo>(list) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }

        public int GetNextSellBatchNumber()
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var local = efdb.DB_Sell.Local;
            if (local.Count() == 0)//缓冲中没有
            {
                var data = LoadEntities(p => true).LastOrDefault();
                if (data == null)//数据库中无数据时
                {
                    return 1;
                }
                else
                {
                    return data.BatchNumber + 1;
                }
            }
            else
            {
                var data = local.OrderBy(p => p.BatchNumber).LastOrDefault();
                return data.BatchNumber + 1;
            }
        }

        public void Add(DB_Sell model, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                model.BatchNumber = GetNextSellBatchNumber();
                AddEntities(model);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public SellBatchInfo QuerySignleBatchInfo(int SellId)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var info = efdb.DB_Sell.Where(p => p.Id == SellId).FirstOrDefault();
            var node = new SellBatchInfo()
            {
                Id = info.Id,
                BatchNumber = info.BatchNumber,
                QuantityDate = info.QuantityDate,
                TranscationObject = info.TransactionObject,
                SellCategory = info.DB_DictionaryInfo.Name,
                QuantityNum = info.QuantityNum,
                RemainderNum = info.RemainderNum,
                QuantitySpend = info.QuantitySpend,
                OtherSpend = info.OtherSpend,
                CurIncome = info.CurIncome,
                CurProfit = info.CurProfit,
                EnterTime = info.EnterTime.ToString("F"),
                Operator = info.DB_Manager.Name
            };
            return node;
        }

        public PagingResults<SellBatchDetailInfo> QuerySellBatchDetaiInfoList(SearchSellBatchDetailFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var query = efdb.DB_SellDetail.Where(p=>p.FK_Sell == filter.SellId).OrderByDescending(p => p.Id).AsQueryable();
            int count = query.Count();
            query = query.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            var list = new List<SellBatchDetailInfo>();
            foreach (var info in query)
            {
                var node = new SellBatchDetailInfo()
                {
                    Id = info.Id,
                    BatchNumber = info.DB_Sell.BatchNumber,
                    TransTime = info.TransTime,
                    TranscationObject = info.TransactionObject,
                    TransReason = info.DB_DictionaryInfo.Name,
                    RemainderNum = info.TransLaterNum,
                    TransNum = info.TransNum.ToString(),
                    TrnasCost = (info.SpendType == 1 ? "+" : "-") + info.TransCost.ToString(),
                    SpendType = info.SpendType == 1 ? "收入" : "支出",
                    EnterTime = info.EnterTime.ToString("F")
                };
                if (info.TransNum != 0)
                {
                    if (node.TransReason == "进货")
                        node.TransNum = "+" + info.TransNum;
                    else if (node.TransReason == "卖出")
                    {
                        node.TransNum = "-" + info.TransNum;
                    }
                }
                else
                    node.TransNum = "0";
                    
                list.Add(node);
            }
            return new PagingResults<SellBatchDetailInfo>(list) { PageSize = filter.PageSize, TotalSize = count, CurrentPageNo = filter.PageNo };
        }

        public void Delete(List<int> list, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_Sell> deleteList = new List<DB_Sell>();
                ISellDetailService _service = new SellDetailService();
                foreach (var Id in list)
                {
                    var info = LoadEntities(p => p.Id == Id).FirstOrDefault();
                    _service.Delete(info.Id, out retMsg);
                    if (retMsg != "")
                        throw new Exception(retMsg);
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
