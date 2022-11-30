using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace Shanti.Api.DispatcherService
{
    public class TcpServer
    {
        private static TcpServer _server;
        TcpListener server;
        TcpClient client;

        private TcpServer()
        {
           
        }

        public static TcpServer getInstance()
        {
            if (_server == null)
                _server = new TcpServer();
            return _server;
        }

        public async Task StartAsync(string ip, int port)
        {
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            try
            {
                server.Start();
                while (true)
                {
                    client = await server.AcceptTcpClientAsync();
                    Console.WriteLine($"Входящее подключение: {client.Client.RemoteEndPoint}");
                }
            }
            finally
            {
                server.Stop();
            }
        }

        public async Task SendTrigger(DispatcherTrigger trigger)
        {
            NetworkStream stream = client.GetStream();
            byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(trigger));
            await stream.WriteAsync(message);
            Console.WriteLine($"Клиенту {client.Client.RemoteEndPoint} отправлены данные");
        }
    }
}
