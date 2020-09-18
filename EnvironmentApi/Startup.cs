using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvironmentApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EnvironmentApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        // 加密解密Token的密钥
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abcdABCD1234abcdABCD1234")),

                        // 是否验证发布者
                        ValidateIssuer = true,
                        // 发布者名称
                        ValidIssuer = "server",

                        // 是否验证订阅者
                        // 订阅者名称
                        ValidateAudience = true,
                        ValidAudience = "client001",

                        // 是否验证令牌有效期
                        ValidateLifetime = true,
                        // 每次颁发令牌，令牌有效时间
                        ClockSkew = TimeSpan.FromMinutes(20)
                    };
                });
            services.AddDbContext<EnvironmentContext>(
                options => options.UseMySQL(Configuration.GetConnectionString("EnvironmentConnection")));
            services.AddScoped<IDeviceStatus, DeviceStatusRepository>();
            services.AddScoped<IEnvironment, EnvironmentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}