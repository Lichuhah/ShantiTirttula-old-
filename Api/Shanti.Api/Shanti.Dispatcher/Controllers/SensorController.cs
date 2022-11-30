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
            CheckTriggers(data);
            Session.AddSensordData(data);
            return "test";
        }

        private void CheckTriggers(List<McSensorData> data)
        {
            foreach(McSensorData sensor in data)
            {
                List<DispatcherTrigger> triggers = Session.Triggers.Where(x => x.SensorId == sensor.SensorId).ToList();
                if (triggers.Any())
                {
                    if (sensor.Value > triggers.First().TriggerValue && !triggers.First().IsCheck)
                    {
                        triggers.First().IsCheck = true;
                        Session.Commands.Add(new McCommand
                        {
                            Pin = triggers.First().Pin,
                            IsPwn = triggers.First().IsPwm,
                            Value = 1
                        });                        
                    }
                    else if(triggers.First().IsCheck)
                    {
                        triggers.First().IsCheck = false;
                        Session.Commands.Add(new McCommand
                        {
                            Pin = triggers.First().Pin,
                            IsPwn = triggers.First().IsPwm,
                            Value = 0
                        });
                    }
                }
            }
        }
       
    }
}
