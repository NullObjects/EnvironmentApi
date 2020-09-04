using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 环境信息接口
    /// </summary>
    public interface IEnvironment : IDBRepository<EnvironmentModel>
    {
    }

    /// <summary>
    /// 环境信息仓储
    /// </summary>
    public class EnvironmentRepository : DBRepository<EnvironmentModel>, IEnvironment
    {
        public EnvironmentRepository(EnvironmentContext context)
        {
            _context = context;
            _dbSet = context.Environment;
        }
    }
}