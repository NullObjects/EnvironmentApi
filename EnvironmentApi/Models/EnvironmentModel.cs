using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 环境信息模型
    /// </summary>
    public class EnvironmentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public DateTime RecordTime { get; set; }
    }
}