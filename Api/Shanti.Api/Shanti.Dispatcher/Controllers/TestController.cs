using Microsoft.AspNetCore.Mvc;

namespace Shanti.Dispatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpPost("sendSensor")]
        public string SendLight([FromBody] SensorData data)
        {
            return data.Value.ToString();
        }
    }
}
