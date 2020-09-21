using Microsoft.EntityFrameworkCore;

namespace EnvironmentApi.Models
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<DeviceStatusModel> DeviceStatus { get; set; }
        public DbSet<EnvironmentModel> Environment { get; set; }


        /// <summary>
        /// 构造Context
        /// </summary>
        /// <param name="options"></param>
        public EnvironmentContext(DbContextOptions<EnvironmentContext> options) : base(options)
        {
        }
    }
}