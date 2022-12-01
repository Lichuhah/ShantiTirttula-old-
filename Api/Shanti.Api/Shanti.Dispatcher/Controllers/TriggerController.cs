using Microsoft.AspNetCore.Mvc;
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
    }
}
