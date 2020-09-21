using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 数据操作泛型接口
    /// </summary>
    /// <typeparam name="T">model类</typeparam>
    public interface IDbRepository<T>
        where T : class
    {
        /// <summary>
        /// 自动提交标志
        /// Default:True
        /// </summary>
        bool AutoCommit { get; set; }

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns></returns>
        T Add(T model);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">数据主键</param>
        /// <returns></returns>
        T Delete(params object[] keyValue);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="keyValue">数据主键</param>
        /// <returns></returns>
        T Select(params object[] keyValue);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Select();

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <returns></returns>
        T Update(T model);
    }

    /// <summary>
    /// 数据操作泛型类
    /// </summary>
    /// <typeparam name="T">model类</typeparam>
    public abstract class DbRepository<T>
        where T : class
    {
        /// <summary>
        /// 数据上下文
        /// </summary>
        protected DbContext DbContext { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        protected DbSet<T> DbSet { get; set; }

        public bool AutoCommit { get; set; } = true;

        public int Commit()
        {
            return DbContext.SaveChanges();
        }

        public T Add(T model)
        {
            DbSet.Add(model);
            if (AutoCommit)
                DbContext.SaveChanges();
            return model;
        }

        public T Delete(params object[] keyValue)
        {
            var data = DbSet.Find(keyValue);
            if (data == null) return null;
            DbSet.Remove(data);
            if (AutoCommit)
                DbContext.SaveChanges();

            return data;
        }

        public T Select(params object[] keyValue)
        {
            return DbSet.Find(keyValue);
        }

        public IEnumerable<T> Select()
        {
            return DbSet;
        }

        public T Update(T model)
        {
            var data = DbSet.Attach(model);
            data.State = EntityState.Modified;
            if (AutoCommit)
                DbContext.SaveChanges();
            return model;
        }
    }
}