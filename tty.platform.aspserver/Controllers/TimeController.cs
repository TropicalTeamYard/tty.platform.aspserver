using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tty.Configs;
using tty.Model;

namespace tty.Controllers
{
    [Produces("application/json")]
    public class TimeController : Controller
    {
        [HttpGet]
        [Route("api/time")]
        public JsonResult Get()
        {
            return TermTime.GetResponce();
        }
    }
}