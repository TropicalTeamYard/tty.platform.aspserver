using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Model;

namespace wejh.platform.aspserver.Controllers
{
    [Produces("application/json")]
    [Route("api/getinfo")]
    public class GetInfoController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit, string type)
        {
#if DEBUG
            return Post(credit, type);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        [HttpPost]
        public JsonResult Post(string credit, string type)
        {
            return UserInfo.GetInfoControl(credit, type);
        }
    }
}