//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Net;
//using System.Threading.Tasks;

//namespace WorldServer.Services
//{


   
// internal sealed class ServerConnectionService
//    {

//        public ILogger logger;
//        private readonly IServiceProvider serviceProvider;

//        public ServerConnectionService(IConfigurationRoot config, ILogger<ServerConnectionService> logger
//            , IServiceProvider serviceProvider)
//        {
//            this.logger = logger;
//            this.serviceProvider = serviceProvider;
//        }

//        public async Task StartAsync()
//        {


//            var Client = new EasyClient();
//            Client.Initialize(new MyReceiveFilter(), request =>
//            {
//                Console.WriteLine(request.Key);
//            });


//            var connected = await Client.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));

//            if (connected)
//            {
//                // Send data to the server
//                Client.Send(Encoding.ASCII.GetBytes("LOGIN kerry"));
//            }

           
//        }

//    }
//}

//    public class MyReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
//    {
//        public MyReceiveFilter()
//        : base(Encoding.ASCII.GetBytes("||")) // two vertical bars as package terminator
//        {
//        }

//        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
//        {
//            throw new NotImplementedException();
//        }

//        // other code you need implement according yoru protocol details
//    }
