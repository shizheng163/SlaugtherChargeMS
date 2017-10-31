using System;
using MSIDAL;
using System.Data.Entity;

namespace MSDAL
{
    /// <summary>
    /// 数据库交互会话，
    /// 如果操作数据库的话直接从这里来操作
    /// </summary>
    public partial class DbSession : IDbSession //代表的是应用程序跟数据库之间的一次会话，也是数据库访问层的统一入口
    {
        /// <summary>
        /// 代表当前应用程序跟数据库的会话内所有的实体变化，更新会数据库
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()  //UintWork单元工作模式
        {
            //调用EF上下文的SaveChanges的方法
            return CurDbContext.SaveChanges();
        }

        public DbContext CurDbContext
        {
            get
            {
                return EfContextFactory.GetCurrentDbContext();
            }
        }

        public IMenusRepository MenusRepository
        {
            get
            {
                return new MenusRepository();
            }
        }

        public IManagerRepository ManagerRepository
        {
            get
            {
                return new ManagerRepository();
            }
        }


        public IUserInfoRepository UserInfoRepository
        {
            get
            {
                return new UserInfoRepository();
            }
        }

        public IDictionaryCategoryRepository DictionaryCategoryRepository
        {
            get
            {
                return new DictionaryCategoryRepository();
            }
        }

        public IDictionaryInfoRepository DictionaryInfoRepository
        {
            get
            {
                return new DictionaryInfoRepository();
            }
        }

        public ISellDetailRepository SellDetailRepository
        {
            get
            {
                return new SellDetailRepository();
            }
        }

        public ISellRepository SellRepository
        {
            get
            {
                return new SellRepository();
            }
        }

        public IButcherProcessRepository ButcherProcessRepository
        {
            get
            {
                return new ButcherProcessRepository();
            }
        }

        public IArrearsDetailRepository ArrearsDetailRepository
        {
            get
            {
                return new ArrearsDetailRepository();
            }
        }

        public ICreditDetailRepository CreditDetailRepository
        {
            get
            {
                return new CreditDetailRepository();
            }
        }
    }
}

