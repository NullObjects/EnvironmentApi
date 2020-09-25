namespace EnvironmentApi.Models
{
    /// <summary>
    /// 用户操作接口
    /// </summary>
    public interface IUser : IDbRepository<UserModel>
    {
    }

    /// <summary>
    /// 用户操作方法
    /// </summary>
    public class UserRepository : DbRepository<UserModel>, IUser
    {
        public UserRepository(EnvironmentContext dbContext)
        {
            base.DbContext = dbContext;
            base.DbSet = dbContext.User;
        }
    }
}