namespace EnvironmentApi.Models
{
    /// <summary>
    /// 设备信息接口
    /// </summary>
    public interface IDeviceStatus : IDbRepository<DeviceStatusModel>
    {
    }

    /// <summary>
    /// 设备信息仓储
    /// </summary>
    public class DeviceStatusRepository : DbRepository<DeviceStatusModel>, IDeviceStatus
    {
        public DeviceStatusRepository(EnvironmentContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.DeviceStatus;
        }
    }
}