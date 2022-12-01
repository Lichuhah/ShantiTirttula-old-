using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Hash;
using Shanti.Dispatcher.Models.Mc;

namespace Shanti.Dispatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        SessionList Sesions;
        public CommandController(SessionList sessionList)
        {
            this.Sesions = sessionList;
        }

        [HttpPost("send")]
        public void AddCommand([FromBody] McCommand command)
        {
            Session session = Sesions.Sessions.FirstOrDefault(x => x.Mc.Serial == command.Serial);
            session.Commands.Add(command);
        }
    }
}
