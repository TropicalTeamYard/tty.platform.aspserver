using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tty.Model;
using tty.Util;

namespace tty.Controllers
{

    [Produces("application/json")]
    [Route("api/bind")]
    public class BindController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit, string bindname, string password, string pid)
        {
#if DEBUG
            return Post(credit, bindname, password, pid);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        [HttpPost]
        public JsonResult Post(string credit, string bindname, string password, string pid)
        {
            return UserInfo.BindControl(credit, bindname, password, pid);
        }

    }
}