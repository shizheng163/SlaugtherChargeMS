/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 14:52:17
*  文件名：SellDetailService
*  说明： 贩卖细节业务层
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
namespace MSBLL
{
    public class SellDetailService : BaseService<DB_SellDetail>, ISellDetailService
    {

        public override void SetCurrentRepository()
        {
            CurrentRepository = db.SellDetailRepository;
        }

        public void AddSellDetail(DB_SellDetail model,out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                var sell = db.SellRepository.LoadEntities(p => p.Id == model.FK_Sell).FirstOrDefault();
                model.TransLaterNum = sell.RemainderNum - model.TransNum;
                if (model.TransLaterNum < 0)
                {
                    retMsg = "剩余货物不足,添加失败!";
                    return;
                }
                if (model.SpendType !=1 && model.SpendType!=2)
                {
                    retMsg = "交易类型选择错误!";
                    return;
                }
                if (model.FK_TranscationReason == "0201")
                {
                    retMsg = "新增交易不允许为进货类交易!";
                    return;
                }
                AddEntities(model);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void DeleteDetail(List<int> IdList, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_SellDetail> deleteList = new List<DB_SellDetail>();
                foreach (var Id in IdList)
                {
                    var info = LoadEntities(p => p.Id == Id).FirstOrDefault();
                    if (info.SpendType == 3)
                    {
                        retMsg = "进货信息不允许删除！";
                        return;
                    }
                    deleteList.Add(info);
                }
                DeleteEntities(deleteList);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void Delete(int FK_Sell, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_SellDetail> deleteList = LoadEntities(p => p.FK_Sell == FK_Sell).ToList();
                DeleteEntities(deleteList);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }
    }
}
