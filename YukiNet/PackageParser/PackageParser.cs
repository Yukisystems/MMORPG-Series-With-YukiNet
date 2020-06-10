///////////////////////////////////////////////////////////////////////////////////////
//  _______ _            _______    _             _       _    _____ _           __  //
// |__   __| |          |__   __|  | |           (_)     | |  / ____| |         / _| //
//    | |  | |__   ___     | |_   _| |_ ___  _ __ _  __ _| | | |    | |__   ___| |_  //
//    | |  | '_ \ / _ \    | | | | | __/ _ \| '__| |/ _` | | | |    | '_ \ / _ \  _| //
//    | |  | | | |  __/    | | |_| | || (_) | |  | | (_| | | | |____| | | |  __/ |   //
//    |_|  |_| |_|\___|    |_|\__,_|\__\___/|_|  |_|\__,_|_|  \_____|_| |_|\___|_|   //
//                                                                                   //
//     The Tutorial Chef : https://youtube.com/c/TheTutorialChef                     //
//     Website:            https://thetutorialchef.com                               //
//     Twitter:            https://twitter.com/thetutorialchef                       //
//     Patreon:            https://www.patreon.com/thetutorialchef                   //
//     Discord:            https://discord.gg/kGrRQJ9                                //
//                                                                                   //
//     Company:            https://yukisystems.com                                   //
//                                                                                   //
//         Copyright by Deadlyviruz aka The Tutorialchef                             //
///////////////////////////////////////////////////////////////////////////////////////
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YukiNet.PackageParser
{
    public class PackageParser : IPackageParser
    {
        private readonly ILogger<PackageParser> logger;
        private readonly PackageParserOptions options;
        private Dictionary<UInt32, Type> packagetypes = new Dictionary<UInt32, Type>();

        public PackageParser(ILogger<PackageParser> logger, IOptions<PackageParserOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;
            ResolvePackages(GetType().Assembly);

            if(this.options?.Assemblies != null)
            {
                RegisterAssemblies(this.options.Assemblies.Distinct().Where(x=>x == GetType().Assembly));
            }

            logger.LogInformation($"Scanned {packagetypes.Count} packagetypes");
        }

        private void RegisterAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            foreach(var assembly in assemblies)
            {
                ResolvePackages(assembly);
            }
        }

        private void ResolvePackages(Assembly assembly)
        {
            var packgeClasses = assembly.GetTypes()
                .Where(x => x.IsSubclassOf(typeof(PackageBase)));

            foreach (var @class in packgeClasses)
            {
                var attribute = @class.GetCustomAttributes(typeof(PackageTypAttributeAttribute), false);
                if (attribute.FirstOrDefault() is PackageTypAttributeAttribute packgeTypAttribute)
                {
                    packagetypes.Add(packgeTypAttribute.PackageType, @class);
                }
            }
        }

        public PackageBase ParsePackageFromStream(BinaryReader reader)
        {
            var packageType = reader.ReadUInt32();

            if (packagetypes.TryGetValue(packageType, out var type))
            {
                var package = Activator.CreateInstance(type) as PackageBase;
                package.DeserialzeFromStream(reader);
                logger.LogInformation($"Received packge from stream type: {package.GetType()}");
                return package;
            }

            throw new InvalidOperationException("Packgetype ist unbekannt");
        }

        public void ParsePackgeToStream(PackageBase package, BinaryWriter writer)
        {
            logger.LogInformation($"write packge from stream type: {package.GetType()}");
            package.SerialzeToStream(writer);
            writer.Flush();
        }
    }


    public class PackageParserOptions
    {
        public IEnumerable<Assembly> Assemblies { get; set; }
    }

    public static class PackageParserDiExtensions
    {
        public static void AddPackageParser(this IServiceCollection serviceDescriptors, IEnumerable<Assembly> assemblies = null)
        {
            serviceDescriptors.AddOptions<PackageParserOptions>()
                .Configure(x =>
                {
                    x.Assemblies = assemblies;
                });

            serviceDescriptors.AddSingleton<IPackageParser, PackageParser>();
        }
    }

}