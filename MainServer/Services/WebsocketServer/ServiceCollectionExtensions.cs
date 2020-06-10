using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainServer.Services.WebsocketServer
{
    public static class ServiceCollectionExtensions
    {

        public static void RegisterWebSocketBehavior<T>(this IServiceCollection col) where T : WebSocketSharp.Server.WebSocketBehavior
        {
            col.AddTransient<T>();
            col.AddTransient<WebSocketSharp.Server.WebSocketBehavior, T>();
        }

    }
}
