using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EnvironmentApi.Models.Authentication
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class LoginRequestDto
    {
        [Required] [JsonProperty("username")] public string UserName { get; set; }

        [Required] [JsonProperty("password")] public string Password { get; set; }
    }
}