using System;
using WebSocketSharp;


namespace WorldServer
{
    public class Program
    {

        private static void Main(string[] args)
        {
            using (var ws = new WebSocket("ws://127.0.0.1:8088/Echo"))
            {
              
                Console.WriteLine("WorldServer Console Log:");
                ws.EmitOnPing = true;
                ws.OnMessage +=  (sender, e) =>
                {
                   var test = !e.IsPing ? e.Data : "Receive a ping.";
                   Console.WriteLine(test);
                    if (e.IsPing)
                    {
                        Console.WriteLine("Ping received, pong responded");
                        
                    }
                    
                    Console.WriteLine("Test:" + e.Data);
                };

                ws.OnOpen += (sender, e) =>
                {
                   // Console.WriteLine("Connection Established");
                    ws.Send("Hello there");
                    
                };
                //OnOpen --> When Connection is established

                //OnClose -> When Connection is closed
                //ConnectAsync
                ws.Connect();
                //Send Async
                ws.Send("TestMessage");

                //wsClose
                //CloseAsync

                Console.ReadLine();
            }

       



            Console.ReadLine();

        }

    }

 




}