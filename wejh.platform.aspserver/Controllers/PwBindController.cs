using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Model;
using wejh.Util;

namespace wejh.platform.aspserver.Controllers
{
    [Produces("application/json")]
    [Route("api/pwbind")]
    public class PwBindController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit, string bindname, string password)
        {
            return Post(credit,bindname,password);
        }

        [HttpPost]
        public JsonResult Post(string credit, string bindname, string password)
        {
            if (credit == null || bindname == null || password == null)
            {
                return new ResponceModel(400, "无效的访问。");
            }
            else
            {
                return UserInfo.PwBind(credit, bindname, password);
            }
        }

    }
}