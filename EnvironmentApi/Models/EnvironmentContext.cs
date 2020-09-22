using Microsoft.EntityFrameworkCore;

namespace EnvironmentApi.Models
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<DeviceStatusModel> DeviceStatus { get; set; }
        public DbSet<EnvironmentModel> Environment { get; set; }

        /// <summary>
        /// 构造Context
        /// </summary>
        /// <param name="options"></param>
        public EnvironmentContext(DbContextOptions<EnvironmentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    Password = "admin@admin",
                    Role = "public::admin"
                });
        }
    }
}