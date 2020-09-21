using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EnvironmentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
    }
}