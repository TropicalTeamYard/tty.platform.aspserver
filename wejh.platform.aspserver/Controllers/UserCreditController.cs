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
    [Route("api/login")]
    public class UserCreditController : Controller
    {
        [HttpGet]
        public JsonResult Login_Get(string username, string password, string devicetype, string devicename)
        {

#if DEBUG
            return Post(username, password, devicetype, devicename);
#else
            return new JsonResult( ResponceModel.GetInstanceBaned());
#endif
            
        }

        [HttpPost]
        public JsonResult Post(string username, string password, string devicetype, string devicename)
        {
            //api访问失败
            if (username == null || password == null || devicetype == null || devicename == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                return UserCredit.Login (username, password, devicetype, devicename);
            }
        }

    }
}