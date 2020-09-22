using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnvironmentApi.Models
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}