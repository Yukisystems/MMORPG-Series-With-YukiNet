using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorldServer.Services
{
    public class ConfigurationService
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public static ConfigurationService CreateInstance()
        {
            return CreateInstance((s) => { });
        }

        public static ConfigurationService CreateInstance(Action<IServiceCollection> handler)
        {
            var instance = new ConfigurationService();

            var descriptors = CreateDefaultSericeDescriptors();
            handler(descriptors);

            instance.ServiceProvider = descriptors.BuildServiceProvider();
            return instance;
        }

        // Server Config file load
        private static IServiceCollection CreateDefaultSericeDescriptors()
        {
            var Configuration = new ConfigurationBuilder()
            .AddJsonFile("config.json")
            
            .Build();

            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddLogging(b => b.AddNLog());
            serviceDescriptors.AddSingleton<IConfigurationRoot>(Configuration);

            return serviceDescriptors;
        }
    }
}
