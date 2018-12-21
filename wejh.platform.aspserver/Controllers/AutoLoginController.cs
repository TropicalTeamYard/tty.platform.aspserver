using System;
using System.Collections.Generic;
using System.Data;
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
    /// 自动登录的API逻辑
    /// </summary>
    [Produces("application/json")]
    [Route("api/autoLogin")]
    public class AutoLoginController : Controller
    {
        [HttpGet]
        public JsonResult Get(string credit)
        {
#if DEBUG
            return Post(credit);
#else
            return ResponceModel.GetInstanceBaned();
#endif
        }

        [HttpPost]
        public JsonResult Post(string credit)
        {
            return UserCredit.AutoLogin(credit);
        }
    }
}