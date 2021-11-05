using System;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace WebSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var exitEvent = new ManualResetEvent(false);
            var url = new Uri("wss://apimarra.azure-api.net/deltaws");
            //var url = new Uri("wss://deltatest1.azurewebsites.net/ws");
            using (var client = new WebsocketClient(url))
            {
                client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                client.ReconnectionHappened.Subscribe(info =>
                    Console.WriteLine($"Reconnection happened, type: {info.Type}"));

                client.MessageReceived.Subscribe(msg => Console.WriteLine($"Message received: {msg}"));
                client.Start();

                Task.Run(() => client.Send("{ message }"));

                exitEvent.WaitOne();
            }
        }
    }
}
