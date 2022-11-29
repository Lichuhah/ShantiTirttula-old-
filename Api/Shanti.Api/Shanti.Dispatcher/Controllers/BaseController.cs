using Microsoft.AspNetCore.Mvc;
using Shanti.Dispatcher.Models.Hash;

namespace Shanti.Dispatcher.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Session Session;
        protected BaseController()
        {
            SessionList List = new SessionList();
            string serial = HttpContext.Request.Headers.First(x => x.Key == "Serial").Value;
            string mac = Request.Headers.First(x => x.Key == "MAC").Value;
            this.Session = List.GetSession(new Models.Mc.McData { Serial = serial, MAC = mac});
        }
    }
}
