namespace EnvironmentApi.Models
{
    /// <summary>
    /// 环境信息接口
    /// </summary>
    public interface IEnvironment : IDbRepository<EnvironmentModel>
    {
    }

    /// <summary>
    /// 环境信息仓储
    /// </summary>
    public class EnvironmentRepository : DbRepository<EnvironmentModel>, IEnvironment
    {
        public EnvironmentRepository(EnvironmentContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Environment;
        }
    }
}