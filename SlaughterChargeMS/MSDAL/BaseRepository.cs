using System;
using System.Data.Entity;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MSDAL
{
    public class BaseRepository<T> where T : class
    {
        //EF上下文的实例保证，线程内唯一
        //实例化EF框架

        //获取的实当前线程内部的上下文实例，而且保证了线程内上下文实例唯一
        private DbContext db = EfContextFactory.GetCurrentDbContext();
        
        
        //添加
        public T AddEntities(T entity)
        {
            db.Entry<T>(entity).State = EntityState.Added;
            //db.SaveChanges();
            return entity;
        }

        //修改
        public bool UpdateEntities(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Modified;
            //db.Entry<T>(entity).State= EntityState.Unchanged;
            //return db.SaveChanges() > 0;
            return true;
        }

        /// <summary>  
        /// 更新指定字段  
        /// </summary>  
        /// <param name="entity">实体，必须包含主键</param>  
        /// <param name="fileds">更新字段数组</param>  
        public bool UpdateEntityFields(T entity, List<string> fileds)
        {
           
            if (entity != null && fileds != null)
            {
                db.Set<T>().Attach(entity);
                var SetEntry = ((IObjectContextAdapter)db).ObjectContext.
                    ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fileds)
                {
                    SetEntry.SetModifiedProperty(t);
                }
                return true;
            }
            return false;
        }

        //删除
        public bool DeleteEntities(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = EntityState.Deleted;
            //return db.SaveChanges() > 0;
            return true;
        }

        //查询
        public IQueryable<T> LoadEntities(Func<T, bool> wherelambda)
        {
            db.Configuration.ProxyCreationEnabled = false;
             return db.Set<T>().AsNoTracking().Where<T>(wherelambda).AsQueryable();
        }
        //2015.11.17
        public IQueryable<T> LoadEntitiesWithoutAsNoTracking(Func<T, bool> wherelambda)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Set<T>().Where<T>(wherelambda).AsQueryable();
        }
        //2015.11.17
        public T LoadEntityiesByIds(int id)
        {
            return db.Set<T>().Find(id);
        }

      

        //分页
        public IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex, out int total,
            Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda)
        {
            var tempData = db.Set<T>().Where<T>(whereLambda);
            total = tempData.Count();

            //排序获取当前页的数据
            if (isAsc)
            {
                tempData = tempData.OrderBy<T, S>(orderByLambda).
                      Skip<T>(pageSize * (pageIndex - 1)).
                      Take<T>(pageSize).AsQueryable();
            }
            else
            {
                tempData = tempData.OrderByDescending<T, S>(orderByLambda).
                     Skip<T>(pageSize * (pageIndex - 1)).
                     Take<T>(pageSize).AsQueryable();
            }
            return tempData.AsQueryable();
        }
    }
}
