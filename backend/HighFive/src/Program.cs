using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using src;

namespace Org.OpenAPITools
{

    public class Program
    {
        private const bool Deployed = false;
        public static void Main(string[] args)
        {
            if (Deployed)
            {
                CreateHostBuilderDeploy(args).Build().Run();
            }
            else
            {
                CreateHostBuilderDevelop(args).Build().Run();
            }

        }
        public static IHostBuilder CreateHostBuilderDeploy(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static IHostBuilder CreateHostBuilderDevelop(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseUrls("http://localhost:5001");
                });
    }
}
