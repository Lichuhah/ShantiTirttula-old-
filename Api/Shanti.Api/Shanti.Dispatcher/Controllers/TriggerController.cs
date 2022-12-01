using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Hash;
using Shanti.Dispatcher.Models.Mc;

namespace Shanti.Dispatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TriggerController : ControllerBase
    {
        SessionList Sesions;
        public TriggerController(SessionList session)
        {
            this.Sesions = session;
        }
        [HttpPost("send")]
        public string SendData([FromBody] DispatcherTrigger trigger)
        {
            Session session = Sesions.Sessions.FirstOrDefault(x => x.Mc.Serial == trigger.Serial);
            trigger.IsCheck = false;
            session.Triggers.Add(trigger);
            return "test";
        }

        [HttpGet("clear")]
        public void Clear()
        {
            foreach(var ses in Sesions.Sessions)
            {
                ses.Triggers.Clear();
            }
        }

        [HttpGet("get")]
        public string list()
        {
            string res = "";
            foreach (var ses in Sesions.Sessions)
            {
                res += JsonConvert.SerializeObject(ses.Triggers) + "\r\n";
            }
            return res;
        }
    }
}
