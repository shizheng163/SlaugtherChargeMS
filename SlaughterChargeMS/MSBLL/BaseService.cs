/****************************************************************************
 * 单元名称：基类逻辑实现类
 * 单元描述：基类逻辑实现类，其它服务类继承于此
 * 作者：
 * 创建日期：2016-04-26 
 * 最后修改：（请最后修改的人填写）
 * 修改日期：XXXX-XX-XX
 * 版本号：Ver 1(每次修改 加 1)
 * (C) 2016 
*****************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using MSDAL;
using MSIDAL;
namespace MSBLL
{
    public abstract class BaseService<T> where T : class,new()
    {
        //在调用这个方法的时候必须给他赋值
        public IBaseRepository<T> CurrentRepository { get; set; }

        //控制savechanges保存次数
        bool saveChanges = true;

        //为了职责单一的原则，将获取线程内唯一实例的DbSession的逻辑放到工厂里面去了
        public IDbSession db = DbSessionFactory.GetCurrentDbSession();

        //基类的构造函数
        public BaseService()
        {
            SetCurrentRepository();  //构造函数里面调用了此设置当前仓储的抽象方法
        }

        //构造方法实现赋值 
        public abstract void SetCurrentRepository();  //约束子类必须实现这个抽象方法

        //添加
        public T AddEntities(T entity)
        {
            //如果在这里操作多个表的话，实现批量的操作
            //调用T对应的仓储来添加
            var addentity = CurrentRepository.AddEntities(entity);

            if (saveChanges) db.SaveChanges();
            return addentity;
        }

        //批量添加多个实体
        public List<T> AddEntities(List<T> entities)
        {
            List<T> result = new List<T>();
            foreach (var entity in entities)
            {
                var res = CurrentRepository.AddEntities(entity);
                result.Add(res);
            }
            db.SaveChanges();
            return result;
        }


        //修改
        public bool UpdateEntities(T entity)
        {
            var updateEntity = CurrentRepository.UpdateEntities(entity);

            if (saveChanges) db.SaveChanges();
            return updateEntity;
        }

        /// <summary>
        /// 多个实体同时修改。
        /// </summary>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public bool UpdateListEntities(List<T> entityList)
        {
            var updateResult = true;
            foreach (var info in entityList)
            {
                updateResult = CurrentRepository.UpdateEntities(info) && updateResult;
            }
            db.SaveChanges();
            return updateResult;
        }
        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="entity">实体，必须包含主键</param>  
        /// <param name="fileds">更新字段数组</param>  
        /// <returns></returns>
        public bool UpdateEntityFields(T entity, List<string> fileds)
        {
            db.CurDbContext.Configuration.ValidateOnSaveEnabled = false;
            var updateEntity = CurrentRepository.UpdateEntityFields(entity, fileds);

            db.SaveChanges();
            db.CurDbContext.Configuration.ValidateOnSaveEnabled = true;
            return updateEntity;
        }

        /// <summary>
        /// 更新多个实体的指定字段
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public bool UpdateListEntityFields(List<T> entityList, List<string> fileds)
        {
            db.CurDbContext.Configuration.ValidateOnSaveEnabled = false;
            var updateResult = true;
            foreach (var info in entityList)
            {
                updateResult = CurrentRepository.UpdateEntityFields(info, fileds) && updateResult;
            }
            db.SaveChanges();
            db.CurDbContext.Configuration.ValidateOnSaveEnabled = true;
            return updateResult;
        }

        //修改
        public bool DeleteEntities(T entity)
        {
            var deleteEntity = CurrentRepository.DeleteEntities(entity);
            db.SaveChanges();
            return deleteEntity;
        }

        //批量删除多个实体
        public bool DeleteEntities(List<T> entities)
        {
            var result = true;
            foreach (var entity in entities)
            {
                result = CurrentRepository.DeleteEntities(entity) && result;
            }
            db.SaveChanges();
            return result;
        }
        //查询
        public IQueryable<T> LoadEntities(Func<T, bool> wherelambda)
        {
            return CurrentRepository.LoadEntities(wherelambda);
        }


        //分页
        public IQueryable<T> LoadPagerEntities<S>(int pageSize, int pageIndex,
             out int total, Func<T, bool> whereLambda, bool isAsc, Func<T, S> orderByLambda)
        {
            return CurrentRepository.LoadPagerEntities(pageSize, pageIndex, out total, whereLambda, isAsc, orderByLambda);
        }

        //开始批量导入前调用
        public void BeginImport()
        {
            //加上这个效率更高，但是其他人的修改会有影响，暂时不开，效率不够再开
            //db.CurDbContext.Configuration.AutoDetectChangesEnabled = false;
            saveChanges = false;
        }

        //批量导入结束时调用
        public void EndImport()
        {
            try
            {
                db.SaveChanges();
            }
            finally
            {
                //db.CurDbContext.Configuration.AutoDetectChangesEnabled = true;
                saveChanges = true;
            }
        }



    }
}
