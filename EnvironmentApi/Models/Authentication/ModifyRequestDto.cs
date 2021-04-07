using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace EnvironmentApi.Models.Authentication
{
    public class ModifyRequestDto
    {
        [Required] [JsonProperty("username")] public string UserName { get; set; }
        [Required] [JsonProperty("email")] public string Email { get; set; }

        [Required]
        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; }

        [Required] [JsonProperty("password")] public string Password { get; set; }
        [Required] [JsonProperty("role")] public string Role { get; set; }
    }
}