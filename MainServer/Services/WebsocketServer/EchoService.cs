using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MainServer.Services.WebsocketServer
{
   public class EchoService : WebSocketBehavior
    {
        private readonly ILogger<EchoService> logger;

        public EchoService(ILogger<EchoService> logger)
        {
            this.logger = logger;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            logger.LogInformation("Echo invoked");
            Send("Echoooo");
        }
    }
}
