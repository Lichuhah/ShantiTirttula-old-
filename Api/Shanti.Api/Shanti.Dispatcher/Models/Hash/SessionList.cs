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
            {
                if (DateTime.UtcNow - session.CreateTime < TimeSpan.FromMinutes(15))
                {
                    return session;
                }
                else
                {
                    return RefreshSession(session);
                }
            }
            else return CreateSession(data);
        }

        public Session CreateSession(McData data)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/Dispatcher/token");
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.Send(request);
            string token = response.Content.ReadAsStringAsync().Result;

            Session session = new Session()
            {
                CreateTime = DateTime.UtcNow,
                LastSendTime = DateTime.UtcNow,
                Token = token,
                Mc = data
            };

            Sessions.Add(session);
            return session;
        }

        public Session RefreshSession(Session oldsession)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/Dispatcher/token");
            request.Content = new StringContent(JsonConvert.SerializeObject(oldsession.Mc), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.Send(request);
            string token = response.Content.ReadAsStringAsync().Result;

            Sessions.Remove(oldsession);
            Session session = new Session()
            {
                CreateTime = DateTime.UtcNow,
                LastSendTime = oldsession.LastSendTime,
                Token = token,
                Mc = oldsession.Mc,
                SensorsData = oldsession.SensorsData
            };

            Sessions.Add(session);
            return session;
        }
    }
}
