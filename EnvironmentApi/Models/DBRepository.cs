using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        protected DbContext _context { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        protected DbSet<T> _dbSet { get; set; }

        public bool AutoCommit { get; set; } = true;

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public T Add(T model)
        {
            _dbSet.Add(model);
            if (AutoCommit)
                _context.SaveChanges();
            return model;
        }

        public T Delete(params object[] keyValue)
        {
            var data = _dbSet.Find(keyValue);
            if (data == null) return null;
            _dbSet.Remove(data);
            if (AutoCommit)
                _context.SaveChanges();

            return data;
        }

        public T Select(params object[] keyValue)
        {
            return _dbSet.Find(keyValue);
        }

        public IEnumerable<T> Select()
        {
            return _dbSet;
        }

        public T Update(T model)
        {
            var data = _dbSet.Attach(model);
            data.State = EntityState.Modified;
            if (AutoCommit)
                _context.SaveChanges();
            return model;
        }
    }
}