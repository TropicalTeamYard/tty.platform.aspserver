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
            if (Config.IsTest)
            {
                return Post(credit);
            }
            else
            {
                return new JsonResult(new ResponceModel(405, "Get方法已禁止"));
            }
        }

        [HttpPost]
        public JsonResult Post(string credit)
        {
            return new JsonResult(ToolUtil.Autologin(credit));
        }
    }
}