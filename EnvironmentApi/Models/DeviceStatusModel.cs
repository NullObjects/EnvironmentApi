using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 设备信息模型
    /// </summary>
    public class DeviceStatusModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public float CPUTemperature { get; set; }
        public float CPUOccupancyRate { get; set; }
        public float RAMOccupancyRate { get; set; }
        public float SDCardOccupancyRate { get; set; }
        public float HDDOccupancyRate { get; set; }
        public DateTime RecordTime { get; set; }
    }
}