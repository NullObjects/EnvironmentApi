using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EnvironmentApi.Models.Authentication
{
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class RegistRequestDto
    {
        [Required] [JsonProperty("username")] public string UserName { get; set; }
        [Required] [JsonProperty("email")] public string Email { get; set; }
        [Required] [JsonProperty("password")] public string Password { get; set; }
        [Required] [JsonProperty("role")] public string Role { get; set; }
    }
}