using System;
using System.Linq;
using System.Collections.Generic;
namespace MSIBLL
{
    public partial interface IBaseService<T> where T : class,new()
    {
        //添加
        T AddEntities(T entity);

        //批量添加多个实体
        List<T> AddEntities(List<T> entities);

        //修改
        bool UpdateEntities(T entity);

        //同时将多个实体提交修改
        bool UpdateListEntities(List<T> entityList);

        //更新指定字段
        bool UpdateEntityFields(T entity, List<string> fileds);

        //更新多个实体的指定字段
        bool UpdateListEntityFields(List<T> entityList, List<string> fileds);

        //删除
        bool DeleteEntities(T entity);

        //批量删除多个实体
        bool DeleteEntities(List<T> entities);
        //查询
        IQueryable<T> LoadEntities(Func<T, bool> wherelambda);


        //分页
        IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex,
            out int total, Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda);

        //开始批量导入前调用
        void BeginImport();

        //批量导入结束时调用
        void EndImport();
    }
}
