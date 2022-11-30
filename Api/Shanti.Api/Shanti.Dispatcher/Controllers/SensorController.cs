using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Hash;
using Shanti.Dispatcher.Models.Mc;
using System.Text;

namespace Shanti.Dispatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : BaseController
    {
        public SensorController(IHttpContextAccessor context, SessionList sessionList) : base(context, sessionList)
        {
        }

        [HttpPost("send")]
        public string SendData([FromBody] McSensorData data)
        {
            Session.SensorsData.Add(data);
            if(DateTime.UtcNow - Session.LastSendTime < TimeSpan.FromSeconds(60))
            {
                SendAverageDataToServer();
            }
            return "test";
        }

        private bool SendAverageDataToServer()
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7184/McData/sendsensor");
            request.Headers.Add("Authorization", Session.Token);
            request.Content = new StringContent(JsonConvert.SerializeObject(new SensorSendData
            {
                Serial = Session.Mc.Serial,
                Value = Session.SensorsData.Average(x=>x.Value),
                Device = Session.SensorsData.Select(x=>x.SensorId).First()
            }), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = client.Send(request);
                string answer = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(answer);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                Session.SensorsData.Clear();
            }
        }
    }
}
