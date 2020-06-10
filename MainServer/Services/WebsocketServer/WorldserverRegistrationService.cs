using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebSocketSharp;
using WebSocketSharp.Server;

using Microsoft.Extensions.DependencyInjection;

namespace MainServer.Services.WebsocketServer
{

    // WorldServer IP in Config im MainServer, Worldserver anmelden am MainServer, Mainserver überprüft ob Worldserver bekannt (anhand port und ip)
    
   public class WorldserverRegistrationService 
    {
        // ReSharper disable once InconsistentNaming
        protected readonly IServiceProvider _serviceProvider;

        public ILogger Logger;
        private WebSocketServer _websocketServer;

        // ReSharper disable once UnusedParameter.Local
        public WorldserverRegistrationService(IConfigurationRoot config, ILogger<WorldserverRegistrationService> logger
            , IServiceProvider serviceProvider)
        {
            Running = false;
            Logger = logger;
            _serviceProvider = serviceProvider;
        }
      
        //  int port;
        public bool Running { get; private set; }

        public void Start()
        {
            _websocketServer = new WebSocketServer("ws://127.0.0.1:8088");

            var endpoints = _serviceProvider.GetServices<WebSocketBehavior>().Select(x=>x.GetType());

            foreach(var endpoint in endpoints)
            {
                _websocketServer.AddWebSocketService(
                   $"/{endpoint.Name.Replace("Service", "")}", () => _serviceProvider.GetRequiredService(endpoint) as WebSocketBehavior);
            }

            _websocketServer.Start();

        }



    }
}

