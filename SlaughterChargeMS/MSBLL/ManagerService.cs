/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 14:52:17
*  文件名：ManagerService
*  说明： 管理员模型处理业务层
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
using SlaughterChargeMS.Utility;
using SlaughterChargeMS.Config;
using CommonModel.Filter;
using CommonModel.Models;
using MSDAL;
namespace MSBLL
{
    public class ManagerService : BaseService<DB_Manager>, IManagerService
    {

        public override void SetCurrentRepository()
        {
            CurrentRepository = db.ManagerRepository;
        }

        public DB_Manager GetManager(string UserId, string password)
        {
            try
            {
                var query = LoadEntities(p => p.LoginName == UserId);
                if (query.Count() == 0)
                    return null;
                var Manager = query.FirstOrDefault();
                //从密码中获取生成密码的盐
                var salt = Manager.Password.Substring(0, BackConfig.SaltLength * 2);
                //取得真实密码
                string realPwd = Manager.Password.Substring(BackConfig.SaltLength * 2);
                //取得输入后的密码加入后的字符
                string inputPwd = SecurityAlgorithm.PBKDF2.GetPassword(password, salt, BackConfig.EncryptPwdLength);

                if (realPwd == inputPwd)
                    return Manager;
                else
                    return null;
            }
            catch
            {
                return null;
            }

        }

        public bool SetPwd(string userID, string pwd, string newpwd, string newpwdok, out string errorMsg)
        {
            errorMsg = "";
            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(pwd) || string.IsNullOrWhiteSpace(newpwd))
            {
                errorMsg = "用户或密码不能为空字符串";
                return false;
            }
            if (newpwd.Trim() != newpwdok.Trim())
            {
                errorMsg = "新密码和确认密码不一致";
                return false;
            }
            if (!IsExtsUserName(userID))
            {
                errorMsg = "用户不存在";
                return false;
            }

            DB_Manager manager = GetManager(userID, pwd);

            if (manager == null)
            {
                errorMsg = "原密码不正确";
                return false;

            }

            manager.Password = SecurityAlgorithm.PBKDF2.Create64PBKDF2Pwd(newpwd);

            return UpdateEntities(manager);
        }

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool IsExtsUserName(string userCode)
        {
            var result = LoadEntities(p => p.LoginName == userCode);
            return result.Any();
        }

        public void AddManager(DB_Manager manager, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                if (LoadEntities(p => p.LoginName == manager.LoginName).Any())
                {
                    retMsg = "登录账户名已存在。";
                    return;
                }
                manager.Password = SecurityAlgorithm.PBKDF2.Create64PBKDF2Pwd(manager.LoginName);
                AddEntities(manager);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public void Delete(List<int> IdList, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                List<DB_Manager> deleteList = new List<DB_Manager>();
                foreach (var Id in IdList)
                {
                    var info = LoadEntities(p => p.Id == Id).FirstOrDefault();
                    if (info.LoginName == "admin") {
                        retMsg = "内置管理员不允许删除.";
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

        public void ResetUserPassword(int UserId)
        {
            var user = LoadEntities(p => p.Id == UserId).FirstOrDefault();
            if (user == null)
                throw new Exception("用户不存在!");
            user.Password = SecurityAlgorithm.PBKDF2.Create64PBKDF2Pwd(user.LoginName);
            UpdateEntities(user);
        }

        public void UpdateManager(DB_Manager manager, out string retMsg)
        {
            retMsg = string.Empty;
            try
            {
                if (LoadEntities(p => p.Id != manager.Id && p.LoginName == manager.LoginName).Any())
                {
                    retMsg = "登录账户名已存在。";
                    return;
                }
                UpdateEntities(manager);
            }
            catch (Exception ex)
            {
                retMsg = ex.Message;
            }
        }

        public PagingResults<ManagerNode> QueryAllManagerWithPager(PagingFilter filter)
        {
            var efdb = EfContextFactory.GetCurrentDbContext() as Entities;
            var query = efdb.DB_Manager.OrderBy(p => p.Id).AsQueryable();
            int count = query.Count();
            query = query.Skip((filter.PageNo - 1) * filter.PageSize).Take(filter.PageSize);
            var list = new List<ManagerNode>();
            foreach(var info in query)
            {
                list.Add(new ManagerNode(info));
            }
            return new PagingResults<ManagerNode>(list) { PageSize = filter.PageSize, CurrentPageNo = filter.PageNo, TotalSize = count };
        }

        public DB_Manager GetManager(int UserId)
        {
            return LoadEntities(p => p.Id == UserId).FirstOrDefault();
        }

        public void SetManagerMenus(DB_Manager manager)
        {
            if (LoadEntities(p => true).Count() == 1)
            {
                var temp = new MenuService().GetAllMenus(1).Select(p=>p.PK_Code);
                manager.Menus = string.Join(",", temp);
            }
            else if (manager.LoginName == "admin")
                manager.Menus = "002,002001";
            else
            {
                var temp = new MenuService().GetAllMenus(1).Select(p => p.PK_Code);
                manager.Menus = string.Join(",", temp);
            }
        }
    }
}
