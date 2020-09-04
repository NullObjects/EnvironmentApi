using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 设备信息接口
    /// </summary>
    public interface IDeviceStatus : IDBRepository<DeviceStatusModel>
    {
    }

    /// <summary>
    /// 设备信息仓储
    /// </summary>
    public class DeviceStatusRepository : DBRepository<DeviceStatusModel>, IDeviceStatus
    {
        public DeviceStatusRepository(EnvironmentContext context)
        {
            _context = context;
            _dbSet = context.DeviceStatus;
        }
    }
}