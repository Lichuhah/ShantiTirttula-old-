using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Hash;

namespace Shanti.Dispatcher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : BaseController
    {
        public CommandController(IHttpContextAccessor context, SessionList sessionList) : base(context, sessionList)
        {
        }

        [HttpGet("get")]
        public string GetCommands()
        {
            string result = "free";
            if (Session.Commands.Any())
            {
                result = JsonConvert.SerializeObject(Session.Commands.First());
                Session.Commands.Clear();
            }
            return result;
        }
    }
}
