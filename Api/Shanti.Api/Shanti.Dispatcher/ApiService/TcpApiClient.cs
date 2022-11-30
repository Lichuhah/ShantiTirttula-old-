using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Hash;
using Shanti.Dispatcher.Models.Mc;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace Shanti.Dispatcher.ApiService
{
    public class TcpApiClient
    {
        private static WebApplication app;
        private static TcpApiClient _client;
        TcpClient client;
        private TcpApiClient()
        {

        }

        public static TcpApiClient getInstance(WebApplication webapp)
        {
            if (_client == null)
            {
                _client = new TcpApiClient();
                app = webapp;
            }
            return _client;
        }

        public async Task StartAsync(string ip, int port)
        {
            client = new TcpClient();
            await client.ConnectAsync(ip, port);

            NetworkStream stream = client.GetStream();
            var responseData = new byte[512];
            string data = string.Empty;
            int bytes=0;
            do
            {
                bytes = await stream.ReadAsync(responseData);
                data = Encoding.UTF8.GetString(responseData, 0, bytes);
                if (data != String.Empty)
                {
                    DispatcherTrigger trigger = JsonConvert.DeserializeObject<DispatcherTrigger>(data);
                    SessionList sessions = (SessionList)app.Services.GetService(typeof(SessionList));
                    Session session = sessions.Sessions.FirstOrDefault(x => x.Mc.Serial == trigger.Serial);
                    trigger.IsCheck = false;
                    session.Triggers.Add(trigger);
                }
            }
            while (client.Connected);
        }
    }
}
