using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                                             .UseWindowsService() //windows
                                                                  //.UseSystemd() //linux
                                             .ConfigureServices((hostContext, services) =>
                                             {
                                                 services.AddHostedService<Worker>();
                                             });
    }
}
