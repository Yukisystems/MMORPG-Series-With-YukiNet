using MainServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using YukiNet.PackageParser;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var configServices = ConfigurationService.CreateInstance(serviceDescriptors =>
            {
                serviceDescriptors.AddTransient<IPackageParser, PackageParser>();
                // Auth Service Handler
            });


            var client = ActivatorUtilities.CreateInstance<TestClient>(configServices.ServiceProvider);
            client.Execute();

            Console.ReadLine();
        }
    }
}
