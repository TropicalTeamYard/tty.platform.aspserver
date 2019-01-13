using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tty.Controllers
{
    [Produces("application/json")]
    [Route("")]
    public class HelloController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return $"welcome to hello world";
        }
    }
}