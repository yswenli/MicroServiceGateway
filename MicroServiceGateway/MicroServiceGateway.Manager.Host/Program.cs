using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroServiceGateway.Manager.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {

#if DEBUG
            //ManagerService.GeneratCode();
            ManagerService.Start();
            Console.ReadLine();
#endif

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                                             .UseWindowsService()
                                             .ConfigureServices((hostContext, services) =>
                                             {
                                                 services.AddHostedService<Worker>();
                                             });
            }
            else
            {
                return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                                             .UseSystemd()
                                             .ConfigureServices((hostContext, services) =>
                                             {
                                                 services.AddHostedService<Worker>();
                                             });
            }
        }

    }
}
