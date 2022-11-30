using Microsoft.AspNetCore.Mvc;
using Shanti.Dispatcher.Models.Hash;

namespace Shanti.Dispatcher.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Session Session;

        public BaseController(IHttpContextAccessor context, SessionList sessionList)
        {
            string serial = context.HttpContext.Request.Headers.First(x => x.Key == "Serial").Value;
            string mac = context.HttpContext.Request.Headers.First(x => x.Key == "Mac").Value;
            this.Session = sessionList.GetSession(new Models.Mc.McData { Serial = serial, MAC = mac });
        }
    }
}
