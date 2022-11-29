using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Mc;
using System.Net.Http;
using System.Text;

namespace Shanti.Dispatcher.Models.Hash
{
    public class SessionList
    {
        public List<Session> Sessions;
        public SessionList()
        {
            if (Sessions == null) Sessions = new List<Session>();
        }
        public Session GetSession(McData data)
        {
            Session session = Sessions.FirstOrDefault(x => x.Mc.MAC == data.MAC && x.Mc.Serial == data.Serial);
            if (session != null)
                return session;
            else return CreateSession(data);
        }

        public Session CreateSession(McData data)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/Dispatcher/token");
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.SendAsync(request).Result;
            string token = response.Content.ReadAsStringAsync().Result;

            Session session = new Session()
            {
                LastSend = DateTime.UtcNow,
                Token = token,
                Mc = data
            };

            Sessions.Add(session);
            return session;
        }
    }
}
