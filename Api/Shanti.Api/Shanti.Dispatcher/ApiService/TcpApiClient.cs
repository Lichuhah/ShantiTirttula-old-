using Microsoft.AspNetCore.Hosting.Server;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace Shanti.Dispatcher.ApiService
{
    public class TcpApiClient
    {
        private static TcpApiClient _client;
        TcpClient client;
        private TcpApiClient()
        {

        }

        public static TcpApiClient getInstance()
        {
            if (_client == null)
                _client = new TcpApiClient();
            return _client;
        }

        public async Task StartAsync(string ip, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(ip, port);

            //if (client.Connected)
            //    Console.WriteLine($"Подключение с {client.Client.RemoteEndPoint} установлено");
            //else
            //    Console.WriteLine("Не удалось подключиться");

            NetworkStream stream = client.GetStream();
            var responseData = new byte[512];
            string data = string.Empty;
            int bytes=0;
            do
            {
                bytes = await stream.ReadAsync(responseData);
                data = Encoding.UTF8.GetString(responseData, 0, bytes);
                Console.WriteLine(data);
            }
            while (client.Connected);
        }
    }
}
