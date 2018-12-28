using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Configs;
using wejh.Model;
using wejh.Util;

namespace wejh.platform.aspserver.Controllers
{
    /// <summary>
    /// 登录的API逻辑
    /// </summary>
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        [HttpGet]
        public JsonResult Get(string method, string username, string password, string nickname, string devicetype, string newpassword, string credit)
        {
#if DEBUG
            return Post(method, username, password, nickname, devicetype, newpassword, credit);
#else
            return new JsonResult(ResponceModel.GetInstanceBaned());
#endif
            
        }
        [HttpPost]
        public JsonResult Post(string method, string username, string password, string nickname, string devicetype, string newpassword, string credit)
        {
            return UserCredit.Control(method, username, password, nickname, devicetype, newpassword, credit);
        }
    }
}