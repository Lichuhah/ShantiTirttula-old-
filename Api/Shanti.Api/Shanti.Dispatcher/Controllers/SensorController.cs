using Microsoft.AspNetCore.Mvc;
using Shanti.Dispatcher.Models.Mc;

namespace Shanti.Dispatcher.Controllers
{
    public class SensorController : Controller
    {
        [HttpPost]
        public string SendData([FromBody] McSensorData data)
        {
            throw new NotImplementedException();
        }
    }
}
