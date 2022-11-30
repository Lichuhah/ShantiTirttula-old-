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
        public string SendData([FromBody] List<McSensorData> data)
        {
            Session.SensorsData.Add(data);
            if(DateTime.UtcNow - Session.LastSendTime > TimeSpan.FromSeconds(60))
            {
                Session.SendSensorData();
            }
            return "test";
        }
       
    }
}
