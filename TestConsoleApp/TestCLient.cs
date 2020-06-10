using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using YukiNet.PackageParser;
using YukiNet.PackageParser.PackageImplementaions;

namespace TestConsoleApp
{
    public class TestClient
    {
        private readonly ILogger<TestClient> logger;
        private readonly IPackageParser packageParser;

        public TestClient(ILogger<TestClient> logger, IPackageParser packageParser)
        {
            this.logger = logger;
            this.packageParser = packageParser;
        }

        public void Execute()
        {
            TcpClient client = new TcpClient("127.0.0.1", 3456);
            logger.LogInformation("Build test connection");
            var stream = client.GetStream();
            logger.LogInformation("Create Streams");
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);

            logger.LogInformation("Write Login packages");
            packageParser.ParsePackgeToStream(new LoginRequestPackage
            {
                Username="TEST1234",
                Password="GEHEIM",
            }, writer);

            logger.LogInformation("Receive Login response packages");
            var packgeData = packageParser.ParsePackageFromStream(reader);
            logger.LogInformation($"Receive Login response packages TYPE: {packgeData.GetType()} RESULT: {(packgeData as LoginResponsePackage)?.IsValid}");


        }

    }
}
