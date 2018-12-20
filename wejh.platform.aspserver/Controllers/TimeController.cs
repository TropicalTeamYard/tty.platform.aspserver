using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using wejh.Configs;
using wejh.Model;

namespace wejh.platform.aspserver.Controllers
{
    [Produces("application/json")]
    [Route("api/time")]
    public class TimeController : Controller
    {
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                return new ResponceModel(200, "获取时间成功", Config.GetTermTime());
            }
            catch (Exception ex)
            {
                return ResponceModel.GetInstanceError(ex);
            }
        }
    }
}