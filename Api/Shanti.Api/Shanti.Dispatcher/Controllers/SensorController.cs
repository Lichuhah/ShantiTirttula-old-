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
            string result = "";
            try
            {
                CheckTriggers(data);
                if (Session.Commands.Any())
                {
                    result = JsonConvert.SerializeObject(Session.Commands);
                    Session.Commands.Clear();
                }
                Session.AddSensordData(data);
            } catch (Exception ex)
            {
                result += "";
            }
            return result;
        }

        private void CheckTriggers(List<McSensorData> data)
        {
            foreach(McSensorData sensor in data)
            {
                List<DispatcherTrigger> triggers = Session.Triggers.Where(x => x.SensorId == sensor.SensorId).ToList();
                foreach(DispatcherTrigger trigger in triggers)
                {
                    CreateCommand(sensor, trigger);
                }
            }
        }

        private void CreateCommand(McSensorData sensor, DispatcherTrigger trigger)
        {
            if (sensor.Value > trigger.TriggerValue)
            {
                if (!trigger.IsCheck)
                {
                    Session.Commands.Add(new McCommand
                    {
                        Pin = trigger.Pin,
                        IsPwm = trigger.IsPwm,
                        Value = trigger.CommandValue
                    });
                    trigger.IsCheck = !trigger.IsCheck;
                }
            } else
            {
                if (trigger.IsCheck)
                {
                    Session.Commands.Add(new McCommand
                    {
                        Pin = trigger.Pin,
                        IsPwm = trigger.IsPwm,
                        Value = trigger.CommandValue == 1 ? 0 : 1
                    });
                    trigger.IsCheck = !trigger.IsCheck;
                }
            }
        }
       
    }
}
