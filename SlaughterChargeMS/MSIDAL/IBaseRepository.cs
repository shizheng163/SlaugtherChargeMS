using System;
using System.Linq;
using System.Collections.Generic;

namespace MSIDAL
{
    /// <summary>
    /// 基仓储实现的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class,new()
    {
        //添加
        T AddEntities(T entity);

        //修改
        bool UpdateEntities(T entity);
        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="entity">实体，必须包含主键</param>  
        /// <param name="fileds">更新字段数组</param>  
        /// <returns></returns>
        bool UpdateEntityFields(T entity, List<string> fileds);


        //修改
        bool DeleteEntities(T entity);


        //查询
        IQueryable<T> LoadEntities(Func<T, bool> wherelambda);

        /// <summary>
        /// 2015.11.17
        /// </summary> 
        IQueryable<T> LoadEntitiesWithoutAsNoTracking(Func<T, bool> wherelambda);
        /// <summary>
        /// 2015.11.17
        /// </summary> 
        T LoadEntityiesByIds(int id);



        //分页
        IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex,
            out int total, Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda);

    }
}
