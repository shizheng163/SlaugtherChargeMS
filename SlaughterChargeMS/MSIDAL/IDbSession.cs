using System.Data.Entity;

namespace MSIDAL
{
    //添加接口，起约束作用
    public partial interface IDbSession
    {
        DbContext CurDbContext { get; }
        int SaveChanges();
        IMenusRepository MenusRepository { get; }

        IManagerRepository ManagerRepository { get; }

        IUserInfoRepository UserInfoRepository { get; }

        IDictionaryCategoryRepository DictionaryCategoryRepository { get; }

        IDictionaryInfoRepository DictionaryInfoRepository { get; }

        ISellDetailRepository SellDetailRepository { get; }

        ISellRepository SellRepository { get; }

        IButcherProcessRepository ButcherProcessRepository { get; }

        IArrearsDetailRepository ArrearsDetailRepository { get; }

        ICreditDetailRepository CreditDetailRepository { get; }

    }
}
