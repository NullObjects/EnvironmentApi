using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EnvironmentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleToke();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(443,
                            listenOptions => { listenOptions.UseHttps("wwwroot//server.pfx", "Hj147258"); });
                    }).UseStartup<Startup>();
                });

        static void ConsoleToke()
        {
            // 定义用户信息
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestClient"),
                new Claim(JwtRegisteredClaimNames.Email, "66666666666@qq.com"),
            };

            // 和 Startup 中的配置一致
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abcdABCD1234abcdABCD1234"));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "server",
                audience: "client001",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(jwtToken);
        }
    }
}