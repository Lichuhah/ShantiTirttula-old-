using Microsoft.AspNetCore.Mvc;
using Shanti.Dispatcher.Models.Hash;
using Shanti.Dispatcher.Models.Mc;

namespace Shanti.Dispatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly SessionList _SessionList;

        public SensorController(SessionList sessionList)
        {
            _SessionList = sessionList;
        }

        [HttpPost("send")]
        public string SendData([FromBody] McSensorData data)
        {
            //_SessionList = new SessionList();
            string serial = Request.Headers.First(x => x.Key == "Serial").Value;
            string mac = HttpContext.Request.Headers.First(x => x.Key == "Mac").Value;
            Session a = _SessionList.GetSession(new Models.Mc.McData { Serial = serial, MAC = mac });
            throw new NotImplementedException();
        }
    }
}
