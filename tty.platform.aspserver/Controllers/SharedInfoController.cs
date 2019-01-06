using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tty.Model;

namespace tty.platform.aspserver.Controllers
{
    [Produces("application/json")]
    [Route("api/shared")]
    public class SharedInfoController : Controller
    {
        [HttpGet]
        public JsonResult Get(string type,string query)
        {
#if DEBUG
            return Post(type,query);
#else
            return new JsonResult(ResponceModel.GetInstanceBaned());
#endif

        }
        [HttpPost]
        public JsonResult Post(string type,string query)
        {
            return SharedInfo.SharedInfoControl(type,query);
        }
    }
}