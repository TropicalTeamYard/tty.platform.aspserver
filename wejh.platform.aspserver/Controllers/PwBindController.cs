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
            if (credit == null || bindname == null || password == null)
            {
                return new JsonResult(new ResponceModel(400,"无效的访问。"));
            }
            else
            {
                var result = ToolUtil.Autologin(credit);
                if (result.code == 200)
                {
                    //TODO
                    if (bindname == "library")
                    {


                    }
                    else if (bindname == "card")
                    {

                    }
                    else if (bindname == "ycedu")
                    {

                    }
                    else if (bindname == "zjedu")
                    {

                    }
                    else
                    {
                        return new JsonResult(new ResponceModel(400, "绑定类型发生错误。"));
                    }
                }
                else
                {
                    return new JsonResult(new ResponceModel(403, "自动登录已失效，请重新绑定。"));
                }
            }

            return null;
        }
    }
}